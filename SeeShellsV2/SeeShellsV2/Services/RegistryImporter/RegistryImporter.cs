#region copyright
// SeeShells Copyright (c) 2019-2020 Aleksandar Stoyanov, Bridget Woodye, Klayton Killough, 
// Richard Leinecker, Sara Frackiewicz, Yara As-Saidi
// SeeShells is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// SeeShells is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, see <https://www.gnu.org/licenses>
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

namespace SeeShellsV2.Services
{
    public class RegistryImporter : IRegistryImporter
    {
        private IConfigParser Config { get; set; }
        private IShellItemCollection ShellItems { get; set; }
        private IShellItemFactory ShellFactory { get; set; }

        private ISelected Selected { get; set; }

        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private bool registeredCancellation = false;

        public RegistryImporter(
            [Dependency] IConfigParser config,
            [Dependency] IShellItemCollection shellItems,
            [Dependency] IShellItemFactory shellFactory,
            [Dependency] ISelected selected
        )
        {
            Config = config;
            ShellItems = shellItems;
            ShellFactory = shellFactory;
            Selected = selected;
        }

        public Task<(int, int, long)> ImportOnlineRegistry(bool parseAllUsers = false)
        {
            if (!registeredCancellation && Application.Current != null)
            {
                registeredCancellation = true;
                Application.Current.Exit += (_, _) => { tokenSource.Cancel(); };
            }

            SynchronizationContext syncher = SynchronizationContext.Current;

            return Task.Run(() =>
            {
                int parsed = 0, nulled = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                RegistryShellbagRoot root = new RegistryShellbagRoot("Active Registry", "None");

                Selected.Current = root;
                Selected.CurrentEnumerable = root.Children;

                Dictionary<RegistryKeyWrapper, IShellItem> keyShellMappings = new Dictionary<RegistryKeyWrapper, IShellItem>();
                foreach (RegistryKeyWrapper keyWrapper in GetOnlineRegistryKeyIterator(parseAllUsers))
                {
                    if (keyWrapper.Value != null) // Some Registry Keys are null
                    {
                        byte[] buffer = keyWrapper.Value;
                        int off = 0;

                        // extract shell items from registry value
                        while (off + 2 <= buffer.Length && Block.UnpackWord(buffer, off) != 0)
                        {
                            IShellItem parentShellItem = null;

                            //obtain the parent shellitem from the parent registry key (if it exists)
                            if (keyWrapper.Parent != null)
                                if (keyShellMappings.TryGetValue(keyWrapper.Parent, out IShellItem pShellItem))
                                    parentShellItem = pShellItem;

                            IShellItem shellItem = ShellFactory.Create(buffer.Skip(off).ToArray(), parentShellItem);

                            if (shellItem != null)
                            {
                                off += shellItem.Size;
                                parsed++;
                            }
                            else
                            {
                                off += Block.UnpackWord(buffer, off);
                                nulled++;
                                continue;
                            }

                            shellItem.RegistryKey = keyWrapper;
                            shellItem.Parent = parentShellItem;
                            parentShellItem?.Children.Add(shellItem);

                            keyShellMappings.Add(keyWrapper, shellItem);

                            // finish parsing if the parser task was cancelled
                            if (tokenSource.IsCancellationRequested)
                                return (parsed, nulled, stopwatch.ElapsedMilliseconds);

                            // add the shell item to the collection in the caller's thread
                            if (syncher != null)
                            {
                                syncher.Post(delegate 
                                {
                                    ShellItems.Add(shellItem);

                                    // if the shell item has no parent then it belongs to root
                                    if (shellItem.Parent == null)
                                        root.Children.Add(shellItem);
                                }, null);
                            }
                            else
                            {
                                ShellItems.Add(shellItem);

                                // if the shell item has no parent then it belongs to root
                                if (shellItem.Parent == null)
                                    root.Children.Add(shellItem);
                            }
                        }
                    }
                }

                // add the root item to the collection in the caller's thread
                if (syncher != null && root.Children.Count > 0)
                    syncher.Post(delegate { ShellItems.RegistryRoots.Add(root); }, null);
                else if (root.Children.Count > 0)
                    ShellItems.RegistryRoots.Add(root);

                stopwatch.Stop();

                return (parsed, nulled, stopwatch.ElapsedMilliseconds);
            }, tokenSource.Token);
        }

        public Task<(int, int, long)> ImportOfflineRegistry(string registryFilePath)
        {
            if (!registeredCancellation && Application.Current != null)
            {
                registeredCancellation = true;
                Application.Current.Exit += (_, _) => { tokenSource.Cancel(); };
            }

            SynchronizationContext syncher = SynchronizationContext.Current;

            return Task.Run(() =>
            {
                int parsed = 0, nulled = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                RegistryShellbagRoot root = new RegistryShellbagRoot(Path.GetFileName(registryFilePath), registryFilePath);

                Selected.Current = root;
                Selected.CurrentEnumerable = root.Children;

                Dictionary<RegistryKeyWrapper, IShellItem> keyShellMappings = new Dictionary<RegistryKeyWrapper, IShellItem>();
                foreach (RegistryKeyWrapper keyWrapper in GetOfflineRegistryKeyIterator(registryFilePath))
                {
                    if (keyWrapper.Value != null) // Some Registry Keys are null
                    {
                        byte[] buffer = keyWrapper.Value;
                        int off = 0;

                        // extract shell items from registry value
                        while (off + 2 <= buffer.Length && Block.UnpackWord(buffer, off) != 0)
                        {
                            IShellItem parentShellItem = null;

                            //obtain the parent shellitem from the parent registry key (if it exists)
                            if (keyWrapper.Parent != null)
                                if (keyShellMappings.TryGetValue(keyWrapper.Parent, out IShellItem pShellItem))
                                    parentShellItem = pShellItem;

                            IShellItem shellItem = ShellFactory.Create(buffer.Skip(off).ToArray(), parentShellItem);

                            if (shellItem != null)
                            {
                                off += shellItem.Size;
                                parsed++;
                            }
                            else
                            {
                                off += Block.UnpackWord(buffer, off);
                                nulled++;
                                continue;
                            }

                            shellItem.RegistryKey = keyWrapper;
                            shellItem.Parent = parentShellItem;
                            parentShellItem?.Children.Add(shellItem);

                            keyShellMappings.Add(keyWrapper, shellItem);

                            // finish parsing if the parser task was cancelled
                            if (tokenSource.IsCancellationRequested)
                                return (parsed, nulled, stopwatch.ElapsedMilliseconds);

                            // add the shell item to the collection in the caller's thread
                            if (syncher != null)
                            {
                                syncher.Post(delegate
                                {
                                    ShellItems.Add(shellItem);

                                    // if the shell item has no parent then it belongs to root
                                    if (shellItem.Parent == null)
                                        root.Children.Add(shellItem);
                                }, null);
                            }
                            else
                            {
                                ShellItems.Add(shellItem);

                                // if the shell item has no parent then it belongs to root
                                if (shellItem.Parent == null)
                                    root.Children.Add(shellItem);
                            }
                        }
                    }
                }

                // add the root item to the collection in the caller's thread
                if (syncher != null && root.Children.Count > 0)
                    syncher.Post(delegate { ShellItems.RegistryRoots.Add(root); }, null);
                else if (root.Children.Count > 0)
                    ShellItems.RegistryRoots.Add(root);

                stopwatch.Stop();

                return (parsed, nulled, stopwatch.ElapsedMilliseconds);
            }, tokenSource.Token);
        }

        //known Windows locations of NTUSER.dat and USRCLASS.DAT
        //( see https://support.microsoft.com/en-us/help/3048895/error-occurs-during-desktop-setup-and-desktop-location-is-unavailable)
        private static readonly string[] KNOWN_USER_REGISTRY_FILE_LOCATIONS = {
            @"\ntuser.dat", //ntuser is always in base user directory
            @"\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat",
            @"\AppData\Local\Microsoft\Windows\UsrClass.dat"
        };

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
                // // logger.Debug($"SID {storeKey.Name} ASSOCIATED WITH {userOfStore}");

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
                // string message = $"{RegistryFilePath} is not a valid Registry Hive.";
                // logger.Error(ex, message);
                // LogAggregator.Instance.Add(message);
                yield break;
            }

            foreach (string location in Config.GetRegistryLocations())
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

            if (count == 0)
            {
                // string errorMessage = $"Unable to parse hive file {RegistryFilePath}. No Shellbag keys found.";
                // logger.Error(errorMessage);
                // LogAggregator.Instance.Add(errorMessage);
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
                foreach (string usernameLocation in Config.GetUsernameLocations())
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
                    else
                    {
                        return retval;
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
            catch (Exception ex)
            {
                // // logger.Error(ex, $"Unable to retrieve username from hive {userStore.Name}");
            }

            return retval;
        }

        private string FindOfflineUsername(OfflineRegistryHive hive)
        {
            string retval = string.Empty;
            try
            {
                if (hive.HiveType != OfflineRegistryType.NtUser)
                    return retval;

                //todo refactor Parser.GetUsernameLocations() into key-value pairs for lookup, we have to hardcode key-values otherwise.
                //todo we know of the Desktop value inside the "Shell Folders" location, so naively try this until a better way is found
                Dictionary<string, int> likelyUsernames = new Dictionary<string, int>();
                foreach (string usernameLocation in Config.GetUsernameLocations())
                {
                    //based on the values in '...\Explorer\Shell Folders' the [2] value in the string may not always be the username, but it does appear the most.
                    foreach (OfflineKeyValue value in hive.GetKey(usernameLocation).Values)
                    {
                        //break string up into it's path
                        string[] pathParts = value.ValueData.Split('\\');
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

                //most occurred value is probably the username.
                if (likelyUsernames.Count >= 1)
                {
                    retval = likelyUsernames.OrderByDescending(pair => pair.Value).First().Key;
                }
            }
            catch (Exception ex)
            {
                // logger.Error(ex, "Unable to retrieve username from hive file");
            }

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

            // logger.Trace("**" + subKey);

            foreach (string valueName in subKeys)
            {
                if (valueName.ToUpper() == "ASSOCIATIONS")
                {
                    continue;
                }

                string sk = getSubkeyString(subKey, valueName);
                // logger.Trace("{0}", sk);
                OnlineRegistryKey rkNext;
                try
                {
                    rkNext = rk.OpenSubKey(valueName);
                }
                catch (SecurityException ex)
                {
                    // logger.Warn("ACCESS DENIED: " + ex.Message);
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

                string sk = getSubkeyString(subKey, valueName.KeyName);
                // logger.Trace("{0}", sk);
                OfflineRegistryKey rkNext;
                try
                {
                    rkNext = hive.GetKey(getSubkeyString(rk.KeyPath, valueName.KeyName));
                }
                catch (SecurityException ex)
                {
                    // logger.Warn("ACCESS DENIED: " + ex.Message);
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
            // // logger.Debug("NEW USER SID: " + userStore.Name);
            string userOfStore = FindOnlineUsername(userStore);

            foreach (string location in Config.GetRegistryLocations())
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
                foreach (string userRegistryFilePath in KNOWN_USER_REGISTRY_FILE_LOCATIONS)
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

                            // // logger.Debug($"Parsing Registry hive for {username} at {fullFilePath}");
                            Console.WriteLine($"Parsing Registry hive for {username} at {fullFilePath}");

                            //populate the username since we know the user folder it came from
                            foreach (RegistryKeyWrapper userKey in GetOfflineRegistryKeyIterator(fullFilePath))
                            {
                                userKey.RegistryUser = username;
                                yield return userKey;
                            }
                        }
                        else
                        {
                            // // logger.Debug($"Already visited hive at {fullFilePath}. Skipping");
                        }
                    }
                    else
                    {
                        // // logger.Warn($"Couldnt find registry file {userRegistryFilePath} in User Account {username}. Skipping retriveal of user's shellbags.");
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

        private static string getSubkeyString(string subKey, string addOn)
        {
            return string.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
