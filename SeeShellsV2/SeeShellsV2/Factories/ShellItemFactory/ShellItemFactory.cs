using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Factories
{
    public class ShellItemFactory : IShellItemFactory
    {
        private readonly IList<IShellItemParser> parsers;

        public ShellItemFactory([Dependency] IUnityContainer container)
        {
            parsers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IShellItemParser).IsAssignableFrom(p))
                .Where(q => q.IsClass)
                .Select(r => (IShellItemParser)container.Resolve(r))
                .OrderByDescending(s => s.Priority)
                .ToList();
        }

        public Type GetShellType(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            foreach (var parser in parsers)
                if (parser.CanParse(hive, keyWrapper, value, parent))
                    return parser.ShellItemType;

            return null;
        }

        public IShellItem Create(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            foreach (var parser in parsers)
                if (parser.CanParse(hive, keyWrapper, value, parent))
                    return parser.Parse(hive, keyWrapper, value, parent);

            return null;
        }
    }
}
