using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SeeShellsV2.Data
{
    public class User
    {
        public string SID { get; init; }

        public string Name { get; set; }

        public ObservableCollection<RegistryHive> Hives { get; }
    }
}
