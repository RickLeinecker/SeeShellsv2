using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

using OnlineRegistry = Microsoft.Win32.Registry;
using OnlineRegistryKey = Microsoft.Win32.RegistryKey;

using OfflineRegistryHive = Registry.RegistryHiveOnDemand;
using OfflineRegistryType = Registry.HiveTypeEnum;
using OfflineRegistryKey = Registry.Abstractions.RegistryKey;
using OfflineKeyValue = Registry.Abstractions.KeyValue;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Factories;
using SeeShellsV2.Utilities;

// This code was adapted from the SeeShells V1 project

namespace SeeShellsV2.Services
{
    public class RegistryImporter : IRegistryImporter
    {
        private IConfig Config { get; set; }
        private IUserCollection Users { get; set; }
        private IRegistryHiveCollection RegistryHives { get; set; }
        private IShellItemCollection ShellItems { get; set; }
        private IShellItemFactory ShellFactory { get; set; }

        public event EventHandler RegistryImportBegin;
        public event EventHandler RegistryImportEnd;

        public RegistryImporter(
            [Dependency] IConfig config,
            [Dependency] IUserCollection users,
            [Dependency] IRegistryHiveCollection registryHives,
            [Dependency] IShellItemCollection shellItems,
            [Dependency] IShellItemFactory shellFactory
        )
        {
            Config = config;
            Users = users;
            RegistryHives = registryHives;
            ShellItems = shellItems;
            ShellFactory = shellFactory;
        }

        public (RegistryHive, IEnumerable<IShellItem>) ImportRegistry(bool parseAllUsers = false, bool useOfflineHive = false, string hiveLocation = null)
        {
            RegistryImportBegin?.Invoke(this, EventArgs.Empty);

            IDictionary<RegistryKeyWrapper, IShellItem> keyShellMappings = new Dictionary<RegistryKeyWrapper, IShellItem>();

            IList<IShellItem> parsedItems = new List<IShellItem>();

            User user = null;
            RegistryHive hive = null;

            IEnumerable<RegistryKeyWrapper> registryEnumerator = useOfflineHive ?
                GetOfflineRegistryKeyIterator(hiveLocation) :
                GetOnlineRegistryKeyIterator(parseAllUsers);

            foreach (RegistryKeyWrapper keyWrapper in registryEnumerator)
            {
                if (keyWrapper.Value != null) // Some Registry Keys are null
                {
                    byte[] buffer = keyWrapper.Value;
                    int off = 0;

                    if (user == null)
                    {
                        user = Users.FirstOrDefault(
                            u => u.Name == keyWrapper.RegistryUser && u.SID == keyWrapper.RegistrySID
                        );

                        if (user != null)
                            return (null, null);

                        Users.SynchronizationContext?.Send((_) =>
                        {
                            user = new User { Name = keyWrapper.RegistryUser, SID = keyWrapper.RegistrySID };
                            Users.Add(user);
                        }, null);
                    }

                    if (hive == null)
                    {
                        string name = useOfflineHive ? Path.GetFileName(hiveLocation) : "Live Registry";
                        string pathname = useOfflineHive ? hiveLocation : "N/A";

                        hive = RegistryHives.FirstOrDefault(
                             u => u.Name == name && u.Path == pathname && u.User == user
                         );

                        if (hive != null)
                            return (null, null);

                        RegistryHives.SynchronizationContext?.Send((_) =>
                        {
                            hive = new RegistryHive { Name = name, Path = pathname, User = user };
                            RegistryHives.Add(hive);
                        }, null);

                        Users.SynchronizationContext?.Send((_) =>
                        {
                            user.RegistryHives.Add(hive);
                        }, null);
                    }

                    // extract shell items from registry value
                    while (off + 2 <= buffer.Length && BlockHelper.UnpackWord(buffer, off) != 0)
                    {
                        IShellItem parentShellItem = null;

                        //obtain the parent shellitem from the parent registry key (if it exists)
                        if (keyWrapper.Parent != null)
                            if (keyShellMappings.TryGetValue(keyWrapper.Parent, out IShellItem pShellItem))
                                parentShellItem = pShellItem;

                        byte[] value = buffer.Skip(off).ToArray();
                        IShellItem shellItem = ShellFactory.Create(hive, keyWrapper, value, parentShellItem);

                        if (shellItem != null)
                        {
                            off += shellItem.Size;
                        }
                        else
                        {
                            off += BlockHelper.UnpackWord(buffer, off);

                            // construct a placeholder item if the shellbag cannot be identified
                            shellItem = new UnknownShellItem()
                            {
                                Place = new UnknownPlace()
                                {
                                    Name = "??",
                                    PathName = parentShellItem != null ? Path.Combine(parentShellItem.Place.PathName ?? string.Empty, parentShellItem.Place.Name) : null,
                                },
                                RegistryHive = hive,
                                Value = value,
                                NodeSlot = keyWrapper.NodeSlot,
                                SlotModifiedDate = keyWrapper.SlotModifiedDate,
                                LastRegistryWriteDate = keyWrapper.LastRegistryWriteDate,
                                Parent = parentShellItem
                            };

                            parentShellItem?.Children.Add(shellItem);
                        }

                        keyShellMappings.Add(keyWrapper, shellItem);

                        // add the shell item to the collection
                        ShellItems.Add(shellItem);
                        parsedItems.Add(shellItem);

                        // if the shell item has no parent then it belongs to root
                        if (shellItem.Parent == null)
                            hive.Items.Add(shellItem);
                    }
                }
            }

            RegistryImportEnd?.Invoke(this, EventArgs.Empty);
            return (hive, parsedItems);
        }

        private Dictionary<string, string> sidToUsernameMappings = new Dictionary<string, string>();

        /// <summary>
        /// allows iteration over online registry keys in a foreach loop
        /// </summary>
        private IEnumerable<RegistryKeyWrapper> GetOnlineRegistryKeyIterator(bool parseAllUsers)
        {
            //get relevant online registry
            OnlineRegistryKey store = OnlineRegistry.Users;
            foreach (string userStoreName in store.GetSubKeyNames())
            {
                OnlineRegistryKey storeKey = store.OpenSubKey(userStoreName);
                if (storeKey == null)
                    continue;

                string userOfStore = FindOnlineUsername(storeKey);

                if (parseAllUsers || userOfStore.Equals(Environment.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var wrapper in GetLoggedInUserKeys(storeKey))
                        yield return wrapper;
                }
            }


            if (parseAllUsers)
                foreach (var wrapper in GetLoggedOffUserKeys())
                    yield return wrapper;
        }

        /// <summary>
        /// allows iteration over offline registry keys in a foreach loop
        /// </summary>
        private IEnumerable<RegistryKeyWrapper> GetOfflineRegistryKeyIterator(string registryFilePath)
        {
            int count = 0;
            OfflineRegistryHive hive;
            try
            {
                hive = new OfflineRegistryHive(registryFilePath);
            }
            catch (Exception)
            {
                yield break;
            }

            foreach (string location in Config.ShellbagRootLocations)
            {
                string userOfHive = FindOfflineUsername(hive);
                foreach (RegistryKeyWrapper keyWrapper in IterateOfflineRegistry(hive.GetKey(location), hive, location,
                    null, ""))
                {
                    if (userOfHive != string.Empty)
                    {
                        keyWrapper.RegistryUser = userOfHive;
                    }

                    count++;
                    yield return keyWrapper;
                }

            }
        }

        private string FindOnlineUsername(OnlineRegistryKey userStore)
        {
            string sid = userStore.Name.Split('\\')[1];
            // "_classes" is actually just a user's usrclass.dat, not a seperate user.
            sid = sid.ToUpper().Replace("_CLASSES", "");
            if (sidToUsernameMappings.TryGetValue(sid, out var uName))
            {
                return uName;
            }

            string retval = string.Empty;
            try
            {

                Dictionary<string, int> likelyUsernames = new Dictionary<string, int>();
                foreach (string usernameLocation in Config.UsernameLocations)
                {
                    if (userStore.OpenSubKey(usernameLocation) != null)
                    {
                        //based on the values in '...\Explorer\Shell Folders' the [2] value in the string may not always be the username, but it does appear the most.
                        foreach (string value in userStore.OpenSubKey(usernameLocation).GetValueNames())
                        {
                            string valueData = (string)userStore.OpenSubKey(usernameLocation).GetValue(value);
                            string[] pathParts = valueData.Split('\\');
                            if (pathParts.Length > 2)
                            {
                                string username = pathParts[2]; //usually in the form of C:\Users\username
                                if (!likelyUsernames.ContainsKey(username))
                                {
                                    likelyUsernames[username] = 1;
                                }
                                else
                                {
                                    likelyUsernames[username]++;
                                }
                            }
                        }
                    }
                }

                //most occurred value is probably the username.
                if (likelyUsernames.Count >= 1)
                {
                    retval = likelyUsernames.OrderByDescending(pair => pair.Value).First().Key;
                }

                //add this to our existing list for easy lookup for later potential user hives
                if (retval != null && retval != string.Empty)
                {
                    sidToUsernameMappings.Add(sid, retval);
                }
            }
            catch (Exception)
            { }

            return retval;
        }

        private string FindOfflineUsername(OfflineRegistryHive hive)
        {
            string retval = string.Empty;
            try
            {
                //todo refactor Parser.GetUsernameLocations() into key-value pairs for lookup, we have to hardcode key-values otherwise.
                //todo we know of the Desktop value inside the "Shell Folders" location, so naively try this until a better way is found
                Dictionary<string, int> likelyUsernames = new Dictionary<string, int>();
                foreach (string usernameLocation in Config.UsernameLocations)
                {
                    if (hive.GetKey(usernameLocation) == null)
                        continue;

                    Func<string, string> extractUsername = (string path) =>
                    {
                        if (!Path.IsPathFullyQualified(path))
                            return null;

                        //break string up into it's path
                        string[] pathParts = path.Split('\\');
                        if (pathParts.Length > 2 && pathParts[1] == "Users")
                        {
                            return pathParts[2]; //usually in the form of C:\Users\username
                        }

                        return null;
                    };

                    //based on the values in '...\Explorer\Shell Folders' the [2] value in the string may not always be the username, but it does appear the most.
                    foreach (OfflineKeyValue value in hive.GetKey(usernameLocation).Values)
                    {
                        var username = extractUsername(value.ValueData);

                        if (username != null)
                        {
                            if (!likelyUsernames.ContainsKey(username))
                                likelyUsernames[username] = 1;
                            else
                                likelyUsernames[username]++;
                        }

                        username = extractUsername(value.ValueName);

                        if (username != null)
                        {
                            if (!likelyUsernames.ContainsKey(username))
                                likelyUsernames[username] = 1;
                            else
                                likelyUsernames[username]++;
                        }
                    }
                }

                //most occurred value is probably the username.
                if (likelyUsernames.Count >= 1)
                {
                    retval = likelyUsernames.OrderByDescending(pair => pair.Value).First().Key;
                }
            }
            catch (Exception)
            { }

            return retval;
        }

        /// <summary>
        /// Recursively iterates over the a registry key and its subkeys for enumerating all values of the keys and subkeys
        /// </summary>
        /// <param name="rk">The root registry key to start iterating over</param>
        /// <param name="subKey">the path of the first subkey under the root key</param>
        /// <param name="parent">The Parent Key of the Registry Key currently being iterated. Can be null</param>
        /// <returns></returns>
        private static IEnumerable<RegistryKeyWrapper> IterateOnlineRegistry(OnlineRegistryKey rk, string subKey, RegistryKeyWrapper parent)
        {
            if (rk == null)
                yield break;

            string[] subKeys = rk.GetSubKeyNames();

            foreach (string valueName in subKeys)
            {
                if (valueName.ToUpper() == "ASSOCIATIONS")
                {
                    continue;
                }

                string sk = GetSubkeyString(subKey, valueName);
                OnlineRegistryKey rkNext;
                try
                {
                    rkNext = rk.OpenSubKey(valueName);
                }
                catch (SecurityException)
                {
                    continue;
                }

                RegistryKeyWrapper rkNextWrapper = null;

                //shellbags only have their numerical identifer for the value name, not a shellbag otherwise
                bool isNumeric = int.TryParse(valueName, out _);
                if (isNumeric)
                {
                    byte[] byteVal = (byte[])rk.GetValue(valueName);
                    yield return rkNextWrapper = new RegistryKeyWrapper(rkNext, byteVal, parent);
                }

                foreach (var wrapper in IterateOnlineRegistry(rkNext, sk, rkNextWrapper))
                    yield return wrapper;
            }
        }

        /// <summary>
        /// Recursively iterates over the a registry key and its subkeys for enumerating all values of the keys and subkeys
        /// </summary>
        /// <param name="rk">the root registry key to start iterating over</param>
        /// <param name="hive">the offline registry hive</param>
        /// <param name="subKey">the path of the first subkey under the root key</param>
        /// <param name="indent"></param>
        /// <param name="path_prefix">the header to the current root key, needed for identification of the registry store</param>
        /// <returns></returns>
        private static IEnumerable<RegistryKeyWrapper> IterateOfflineRegistry(OfflineRegistryKey rk, OfflineRegistryHive hive, string subKey, RegistryKeyWrapper parent, string path_prefix)
        {
            if (rk == null)
            {
                yield break;
            }

            foreach (OfflineRegistryKey valueName in rk.SubKeys)
            {
                if (valueName.KeyName.ToUpper() == "ASSOCIATIONS")
                {
                    continue;
                }

                string sk = GetSubkeyString(subKey, valueName.KeyName);
                OfflineRegistryKey rkNext;
                try
                {
                    rkNext = hive.GetKey(GetSubkeyString(rk.KeyPath, valueName.KeyName));
                }
                catch (SecurityException)
                {
                    continue;
                }

                string path = path_prefix;
                RegistryKeyWrapper rkNextWrapper = null;

                bool isNumeric = int.TryParse(valueName.KeyName, out _);
                if (isNumeric)
                {
                    OfflineKeyValue rkValue = null;
                    try
                    {
                        rkValue = rk.Values.First(val => val.ValueName == valueName.KeyName);
                    }
                    catch (InvalidOperationException) { }

                    if (rkValue == null)
                        continue;

                    byte[] byteVal = rkValue.ValueDataRaw;
                    yield return rkNextWrapper = new RegistryKeyWrapper(rkNext, byteVal, hive, parent);
                }

                foreach (var wrapper in IterateOfflineRegistry(rkNext, hive, sk, rkNextWrapper, path))
                    yield return wrapper;
            }
        }

        private IEnumerable<RegistryKeyWrapper> GetLoggedInUserKeys(OnlineRegistryKey userStore)
        {
            string userOfStore = FindOnlineUsername(userStore);

            foreach (string location in Config.ShellbagRootLocations)
            {
                foreach (RegistryKeyWrapper keyWrapper in IterateOnlineRegistry(userStore.OpenSubKey(location), location, null))
                {
                    if (userOfStore != string.Empty)
                    {
                        keyWrapper.RegistryUser = userOfStore;
                    }

                    yield return keyWrapper;
                }
            }
        }

        /// <summary>
        /// Obtains the Relevant Registry keys from users who are currently not signed in.
        /// Uses the <seealso cref="OfflineRegistryReader"/> to read logged out uses due to their stores not being
        /// loaded into the active registry when not signed in.
        /// <see cref="https://stackoverflow.com/a/11433344"/>
        /// </summary>
        /// <returns>An iterator for Registry Keys from the offline users on the machine</returns>
        private IEnumerable<RegistryKeyWrapper> GetLoggedOffUserKeys()
        {
            HashSet<string> seenRegistryHives = new HashSet<string>() { string.Empty };

            //navigate to root users directory
            string rootUserDirectory = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).ToString();
            string[] userDirectories = Directory.GetDirectories(rootUserDirectory);

            //attempt to navigate into each user directory, cancel and log if we are blocked by security exceptions
            foreach (string userDirectory in userDirectories)
            {
                string username = userDirectory.Split('\\').Last();

                //check if the files exist in known locations of NTUSER.dat and USRCLASS.DAT 
                foreach (string userRegistryFilePath in Config.UserRegistryLocations)
                {
                    string fullFilePath = userDirectory + userRegistryFilePath;
                    if (File.Exists(fullFilePath))
                    {
                        //check if we've seen this file (in case two directories pointed to one file)
                        string hash = GetFileSha256(fullFilePath);
                        if (!seenRegistryHives.Contains(hash))
                        {
                            seenRegistryHives.Add(hash);
                            //resolve this user's SID, if its the SID of a user who's logged in, skip.
                            //todo

                            Console.WriteLine($"Parsing Registry hive for {username} at {fullFilePath}");

                            //populate the username since we know the user folder it came from
                            foreach (RegistryKeyWrapper userKey in GetOfflineRegistryKeyIterator(fullFilePath))
                            {
                                userKey.RegistryUser = username;
                                yield return userKey;
                            }
                        }
                    }
                }
            }
        }

        private static string GetFileSha256(string filePath)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    using (FileStream stream = File.OpenRead(filePath))
                    {
                        byte[] hash = sha256.ComputeHash(stream);
                        var hashString = hash.Aggregate(string.Empty, (current, b) => current + b.ToString("x2"));
                        // // logger.Debug($"Hash of {filePath} is {hashString}");
                        return hashString;
                    }
                }
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }

        private static string GetSubkeyString(string subKey, string addOn)
        {
            return string.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
