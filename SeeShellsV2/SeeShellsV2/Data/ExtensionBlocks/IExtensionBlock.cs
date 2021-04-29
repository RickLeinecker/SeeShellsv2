using System;
using System.Collections.Generic;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// An extension block commonly found on shellbags
    /// </summary> 
    public interface IExtensionBlock
    {
        /// <summary>
        /// Size of the extension block
        /// </summary>
        ushort Size { init; get; }

        /// <summary>
        /// Version of the extension block
        /// </summary>
        ushort ExtensionVersion { init; get; }

        /// <summary>
        /// extension block signature unique to each type of extension block
        /// </summary>
        uint Signature { init; get; }

        /// <summary>
        /// all properties that exist in the ExtensionBlock in Key-Value Format.
        /// </summary>
        IReadOnlyDictionary<string, object> Fields { get; }
    }
}