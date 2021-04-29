using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// The root of a shellbag tree from a registry hive
    /// </summary>
    public class RegistryHive : IComparable<RegistryHive>
    {
        /// <summary>
        /// Name of the source registry hive
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// absolute path to the registry hive
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// User who owns the registry hive
        /// </summary>
        public User User { get; init; }

        /// <summary>
        /// Top level children of the shellbag root
        /// </summary>
        public AsyncObservableCollection<Place> Places { get; private init; }

        /// <summary>
        /// Top level children of the shellbag root
        /// </summary>
        public AsyncObservableCollection<IShellItem> Items { get; private init; }

        public ICollectionView Drives => _drives.View;
        public ICollectionView Devices => _devices.View;
        public ICollectionView NetworkLocations => _networkLocations.View;

        private CollectionViewSource _drives = new CollectionViewSource();
        private CollectionViewSource _devices = new CollectionViewSource();
        private CollectionViewSource _networkLocations = new CollectionViewSource();

        public RegistryHive()
        {
            Places = new AsyncObservableCollection<Place>();
            Items = new AsyncObservableCollection<IShellItem>();

            _drives.Source = Places;
            _drives.Filter += (o, e) => e.Accepted = e.Item is Drive || e.Item is RemovableDrive;

            _devices.Source = Places;
            _devices.Filter += (o, e) => e.Accepted = e.Item is RemovableDevice;

            _networkLocations.Source = Places;
            _networkLocations.Filter += (o, e) => e.Accepted = e.Item is NetworkLocation;
        }

        public int CompareTo(RegistryHive other)
        {
            return (User, Path, Name).CompareTo((other.User, other.Path, other.Name));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
