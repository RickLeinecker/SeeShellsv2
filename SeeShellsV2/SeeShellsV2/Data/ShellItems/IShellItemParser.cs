using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// An object that is used to parse ShellItems from raw shellbag byte strings
    /// </summary>
    public interface IShellItemParser
    {
        /// <summary>
        /// The type of shell item that this parser parses
        /// </summary>
        public Type ShellItemType { get; }

        /// <summary>
        /// The parsing priority of this parser.
        /// The <see cref="Factories.IShellItemFactory"/> will attempt
        /// to parse shellbag registry keys using parsers with higher priorities first.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Check if a given registry value can be parsed by this parser
        /// </summary>
        /// <param name="keyWrapper">a registry key and value</param>
        /// <param name="parent">the parent shell item of buf</param>
        /// <returns>true if the given registry value can be parsed by this parser</returns>
        bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null);

        /// <summary>
        /// Parse the given registry key into a new shell item
        /// </summary>
        /// <param name="keyWrapper">a registry key and value</param>
        /// <param name="parent">the parent shell item of buf</param>
        /// <returns>a new shell item or null if buf cannot be parsed by this parser</returns>
        IShellItem Parse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null);
    }
}
