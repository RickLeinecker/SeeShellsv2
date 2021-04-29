using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// An object representing the User account of a registry hive owner.
    /// </summary>
    public class User : IComparable<User>
    {
        /// <summary>
        /// Unique Windows Security ID of the user. This is usually included in the name of the user's two user profile hives.
        /// </summary>
        public string SID { get; init; }

        /// <summary>
        /// The user's username
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A list of registry hives associated with the user
        /// </summary>
        public AsyncObservableCollection<RegistryHive> RegistryHives { get; private init; }

        public User()
        {
            RegistryHives = new AsyncObservableCollection<RegistryHive>();
        }

        public int CompareTo(User other)
        {
            // It is very unlikely that two users will have the same SID (getting struck by lightning is probably more likely).
            return SID.CompareTo(other.SID);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
