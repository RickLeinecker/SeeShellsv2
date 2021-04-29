using System;

using LiveRegistryKey = Microsoft.Win32.RegistryKey;
using LiveRegistryHive = Microsoft.Win32.RegistryHive;

using OfflineRegistryHive = Registry.RegistryHiveOnDemand;
using OfflineRegistryKey = Registry.Abstractions.RegistryKey;
using OfflineKeyValue = Registry.Abstractions.KeyValue;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// A wrapper around an extracted registry key. Inherited from the V1 team.
    /// </summary>
    public class RegistryKeyWrapper
    {
        public byte[] Value { get; internal set; }
        public string RegistryUser { get; internal set; }
        public string RegistrySID { get; internal set; }
        public string RegistryPath { get; internal set; }
        public DateTime LastRegistryWriteDate { get; internal set; }

        public int? NodeSlot { get; internal set; }
        public DateTime? SlotModifiedDate { get; internal set; }

        public RegistryKeyWrapper Parent { get; }

        public RegistryKeyWrapper(byte[] value)
        {
            Value = value;
            RegistryUser = string.Empty;
            RegistryPath = string.Empty;
            RegistrySID = string.Empty;
            LastRegistryWriteDate = DateTime.MinValue;
            NodeSlot = null;
            SlotModifiedDate = null;
        }

        public RegistryKeyWrapper(byte[] value, string registryUser, string registryPath) : this(value)
        {
            this.RegistryUser = registryUser;
            this.RegistryPath = registryPath;
        }

        /// <summary>
        /// Adapts a ShellBag RegistryKey to a common standard for retrieval of important information independent of key retrieval methodologies
        /// </summary>
        /// <param name="registryKey">A Registry Key associated with a Shellbag, retrieved via Win32 API </param>
        /// <param name="keyValue">The Value of a Registry key containing Shellbag information. Found in the Parent of the registryKey being inspected</param>
        /// <param name="parent">The parent of the currently inspected registryKey. Can be null.</param>
        public RegistryKeyWrapper(LiveRegistryKey registryKey, byte[] keyValue, RegistryKeyWrapper parent = null) : this(keyValue)
        {
            Parent = parent;
            RegistryPath = registryKey.Name;
            AdaptWin32Key(registryKey);
        }

        /// <summary>
        /// Adapts a ShellBag RegistryKey to a common standard for retrieval of important information independent of key retrieval methodologies
        /// </summary>
        /// <param name="registryKey">A Registry Key associated with a Shellbag, retrieved from a offline registry reader API</param>
        /// <param name="keyValue">The Value of a Registry key containing Shellbag information. Found in the Parent of the registryKey being inspected</param>
        /// <param name="parent">The parent of the currently inspected registryKey. Can be null.</param>
        public RegistryKeyWrapper(OfflineRegistryKey registryKey, byte[] keyValue, OfflineRegistryHive hive, RegistryKeyWrapper parent = null) : this(keyValue)
        {
            Parent = parent;
            RegistryPath = registryKey.KeyPath;
            AdaptOfflineKey(registryKey, hive);
        }

        private void AdaptWin32Key(LiveRegistryKey registryKey)
        {
            //obtain SID and Username(?)

            //HKEY USERS registry is {hivename}\UserSID\....
            string UserSID = registryKey.Name.Split('\\')[1];

            // "_classes" is actually just a user's usrclass.dat, not a seperate user.
            UserSID = UserSID.ToUpper().Replace("_CLASSES", "");
            RegistrySID = UserSID;

            //if we dont know the username, default to the SID. 
            RegistryUser = RegistrySID;

            //obtain NodeSlot (Shellbag Path in registry)
            SlotModifiedDate = DateTime.MinValue;
            NodeSlot = null;

            if (registryKey.GetValue("NodeSlot") is int slot)
            {
                NodeSlot = slot;
                string nodeSlotPath = string.Format("{0}{1}\\{2}", registryKey.Name.Substring(0, registryKey.Name.IndexOf("BagMRU", StringComparison.Ordinal)), "Bags", slot);

                if (registryKey.Name.StartsWith("HKEY_USERS"))
                {
                    SlotModifiedDate = RegistryHelper.GetDateModified(LiveRegistryHive.Users, nodeSlotPath.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;
                }
            }

            //obtain the date the registry last wrote this key
            LastRegistryWriteDate = RegistryHelper.GetDateModified(LiveRegistryHive.Users, registryKey.Name.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;
        }

        private void AdaptOfflineKey(OfflineRegistryKey registryKey, OfflineRegistryHive hive)
        {
            //obtain SID and Username(?)

            //HKEY USERS registry is UserSID\....
            string UserSID = registryKey.KeyPath.Split('\\')[0];

            // "_classes" is actually just a user's usrclass.dat, not a seperate user.
            UserSID = UserSID.ToUpper().Replace("_CLASSES", "");
            RegistrySID = UserSID;

            //if we dont know the username, default to the SID. 
            RegistryUser = RegistrySID;

            //obtain NodeSlot (Shellbag Path in registry)
            SlotModifiedDate = DateTime.MinValue;
            LastRegistryWriteDate = DateTime.MinValue;
            NodeSlot = null;

            string nodeSlotPath = null;
            foreach (OfflineKeyValue kv in registryKey.Values)
            {
                if (kv.ValueName.Equals("NodeSlot"))
                {
                    NodeSlot = int.Parse(kv.ValueData);
                    nodeSlotPath = string.Format("{0}{1}\\{2}", registryKey.KeyPath.Substring(0, registryKey.KeyPath.IndexOf("BagMRU", StringComparison.Ordinal)), "Bags", NodeSlot);
                    break;
                }
            }

            if (nodeSlotPath != null)
            {
                SlotModifiedDate = hive.GetKey(nodeSlotPath).LastWriteTime.Value.LocalDateTime;
            }

            //obtain the date the registry last wrote this key
            LastRegistryWriteDate = registryKey.LastWriteTime.Value.UtcDateTime;
        }
    }
}