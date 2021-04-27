using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Represents a location in Windows File Exporer (such as a Directory, a Network Folder, etc.)
    /// </summary>
    public class Place : IComparable<Place>
    {
        /// <summary>
        /// Type name of this place. Should be overriden by derived classes
        /// </summary>
        public virtual string Type => "Place";

        /// <summary>
        /// The full path string of the directory that contains this place. Append <see cref="Name"/> for the complete path.
        /// </summary>
        public string PathName { get; init; }

        /// <summary>
        /// The name of the Place (this is usually a directory name)
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// ShellItems that contain settings for this place
        /// </summary>
        public IList<IShellItem> Items => _items;

        private IList<IShellItem> _items = new List<IShellItem>();

        public int CompareTo(Place other)
        {
            return (PathName, Name).CompareTo((other.PathName, other.Name));
        }
    }

    public class UnknownPlace : Place { public override string Type => "Unknown"; }
    public class Folder : Place { public override string Type => "Folder"; }
    public class CompressedFolder : Place { public override string Type => "Compressed Folder"; }
    public class SystemFolder : Place { public override string Type => "System Folder"; }
    public class Drive : Place { public override string Type => "Drive"; }
    public class RemovableDrive : Place { public override string Type => "Removable Drive"; }
    public class RemovableDevice : Place { public override string Type => "Removable Device"; }
    public class NetworkLocation : Place { public override string Type => "Network Location"; }
}
