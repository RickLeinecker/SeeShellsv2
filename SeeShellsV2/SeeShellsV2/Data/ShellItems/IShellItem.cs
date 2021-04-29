using System;
using System.Collections.Generic;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// An object that stores parsed shellbag information. Each ShellItem corresponds to one shellbag.
    /// </summary>
    public interface IShellItem : IComparable<IShellItem>
    {
        /// <summary>
        /// A best effort description for a particular ShellItem meant to give the most important / recognizable
        /// piece of information about the Shellitem.
        /// Description can be used to refer to:
        /// <list type="bullet">
        /// <item>filename (e.g. foo.zip)</item>
        /// <item>directory name (e.g. "C:\" for the C drive directory)</item>
        /// <item>GUID (or the known correspondence of a GUID)</item>
        /// </list> 
        /// </summary>
        string Description { init; get; }

        /// <summary>
        /// Place refered to by the shell item.
        /// </summary>
        Place Place { init; get; }

        RegistryHive RegistryHive { init; get; }

        byte[] Value { init; get; }

        int? NodeSlot { init; get; }

        DateTime? SlotModifiedDate { init; get; }

        DateTime LastRegistryWriteDate { init; get; }

        /// <summary>
        /// Parent of this shell item
        /// </summary>
        IShellItem Parent { init; get; }

        /// <summary>
        /// Children of this shell item
        /// </summary>
        IList<IShellItem> Children { get; }

        /// <summary>
        /// Tells the Size of the entire ShellItem. Includes the two bytes to represent the sie parameter.
        /// If Size is 0 it means the rest of the shell item is empty. (e.g. 2 byte array with the size value of 0x00 is an empty shellitem) 
        /// </summary>
        ushort Size { init; get; }

        /// <summary>
        /// Unique byte identifier that represents a shell item type.
        /// Has no effect on signature based Shell items.
        /// See <seealso cref="TypeName"/>, <seealso cref="SubTypeName"/> for a Human readable interpretation of the Shell Item.
        /// </summary>
        byte Type { init; get; }

        /// <summary>
        /// Unique uint identifier that represents a shell item type.
        /// Only present on signature based shell items
        /// See <seealso cref="TypeName"/>, <seealso cref="SubTypeName"/> for a Human readable interpretation of the Shell Item.
        /// </summary>
        uint Signature { init; get; }

        /// <summary>
        /// Human readable interpretation of <see cref="Type"/>
        /// </summary>
        string TypeName { init; get; }

        /// <summary>
        /// Human readable interpretation of <see cref="Type"/>
        /// </summary>
        string SubtypeName { init; get; }

        /// <summary>
        /// all properties that exist in the ShellItem in Key-Value Format.
        /// </summary>
        IReadOnlyDictionary<string, object> Fields { get; }

        /// <summary>
        /// List of <see cref="IExtensionBlock"/> instances associated with the shell item
        /// </summary>
        IReadOnlyCollection<IExtensionBlock> ExtensionBlocks { init; get; }
    }
}
