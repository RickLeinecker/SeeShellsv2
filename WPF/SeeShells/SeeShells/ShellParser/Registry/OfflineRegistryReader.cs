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
using System.Linq;
using System.Windows;
using Registry;
using Registry.Abstractions;
using SeeShells.UI;

namespace SeeShells.ShellParser.Registry
{
    /// <summary>
    /// This class is used to read an offline regisry hive file.
    /// Using the Registry API, from EricZimmerman, it reads registry values from an hive file. 
    /// </summary>
    public class OfflineRegistryReader : IRegistryReader
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        IConfigParser Parser { get; }
        private String RegistryFilePath;


        public OfflineRegistryReader(IConfigParser parser, String registryFilePath)
        {
            Parser = parser;
            RegistryFilePath = registryFilePath;
        }

        public List<RegistryKeyWrapper> GetRegistryKeys()
        {
            List<RegistryKeyWrapper> retList = new List<RegistryKeyWrapper>();
            RegistryHiveOnDemand hive;
            try
            {
                hive = new RegistryHiveOnDemand(RegistryFilePath);

            }
            catch (Exception ex)
            {
                string message = $"{RegistryFilePath} is not a valid Registry Hive.";
                logger.Error(ex, message);
                LogAggregator.Instance.Add(message);
                return retList;
            }

            foreach (string location in Parser.GetRegistryLocations())
            {
                    string userOfHive = FindOfflineUsername(hive);
                try
                {
                    foreach (RegistryKeyWrapper keyWrapper in IterateRegistry(hive.GetKey(location), hive, location,
                        null, ""))
                    {
                        if (userOfHive != string.Empty)
                        {
                            keyWrapper.RegistryUser = userOfHive;
                        }

                        retList.Add(keyWrapper);
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Unable to retrieve keys in {RegistryFilePath} at {location}";
                    logger.Error(ex, errorMessage);
                }
            }

            if (retList.Count == 0)
            {
                string errorMessage = $"Unable to parse hive file {RegistryFilePath}. No Shellbag keys found.";
                logger.Error(errorMessage);
                LogAggregator.Instance.Add(errorMessage);
            }

            return retList;
        }

        private string FindOfflineUsername(RegistryHiveOnDemand hive)
        {
            string retval = string.Empty;
            try
            {
                if (hive.HiveType != HiveTypeEnum.NtUser)
                    return retval;

                //todo refactor this List into key-value pairs for lookup, we have to hardcode key-values otherwise.
                List<string> usernameLocations = Parser.GetUsernameLocations();

                //todo we know of the Desktop value inside the "Shell Folders" location, so naively try this until a better way is found
                Dictionary<string, int> likelyUsernames = new Dictionary<string, int>();
                foreach (string usernameLocation in usernameLocations)
                {
                    //based on the values in '...\Explorer\Shell Folders' the [2] value in the string may not always be the username, but it does appear the most.
                    foreach (KeyValue value in hive.GetKey(usernameLocation).Values)
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
                logger.Error(ex, "Unable to retrieve username from hive file");
            }

            return retval;
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
        static List<RegistryKeyWrapper> IterateRegistry(RegistryKey rk, RegistryHiveOnDemand hive, string subKey, RegistryKeyWrapper parent, string path_prefix)
            {
                List<RegistryKeyWrapper> retList = new List<RegistryKeyWrapper>();
                if (rk == null)
                {
                    return retList;
                }

                foreach (RegistryKey valueName in rk.SubKeys)
                {
                    if (valueName.KeyName.ToUpper() == "ASSOCIATIONS")
                    {
                        continue;
                    }

                string sk = getSubkeyString(subKey, valueName.KeyName);
                    logger.Trace("{0}", sk);
                    RegistryKey rkNext;
                    try
                    {
                        rkNext = hive.GetKey(getSubkeyString(rk.KeyPath, valueName.KeyName));
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        logger.Warn("ACCESS DENIED: " + ex.Message);
                        continue;
                    }

                    string path = path_prefix;
                    RegistryKeyWrapper rkNextWrapper = null;

                    bool isNumeric = int.TryParse(valueName.KeyName, out _);
                    if (isNumeric)
                    {
                        try
                        {
                            KeyValue rkValue = rk.Values.First(val => val.ValueName == valueName.KeyName);
                            byte[] byteVal = rkValue.ValueDataRaw;
                            rkNextWrapper = new RegistryKeyWrapper(rkNext, byteVal, hive, parent);
                            retList.Add(rkNextWrapper);
                        }

                        catch (OverrunBufferException ex)
                        {
                            logger.Warn("OverrunBufferException: " + valueName.KeyName);
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(valueName.KeyName);
                        }
                    }

                    retList.AddRange(IterateRegistry(rkNext, hive, sk, rkNextWrapper, path));
                }

                return retList;

            }

        static string getSubkeyString(string subKey, string addOn)
        {
            return string.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
