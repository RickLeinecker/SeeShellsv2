using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class User : IComparable<User>
    {
        public string SID { get; init; }

        public string Name { get; set; }

        public AsyncObservableCollection<RegistryHive> RegistryHives { get; private init; }

        public User()
        {
            RegistryHives = new AsyncObservableCollection<RegistryHive>();
        }

        public int CompareTo(User other)
        {
            return SID.CompareTo(other.SID);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
