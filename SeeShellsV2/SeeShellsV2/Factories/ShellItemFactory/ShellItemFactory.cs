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
            // construct an instance of each implementation of IShellItemParser and sort by priority
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
            // iterate over shellitem parsers and attempt to parse with each one, in order of priority.
            foreach (var parser in parsers)
                if (parser.CanParse(hive, keyWrapper, value, parent))
                    return parser.ShellItemType;

            return null;
        }

        public IShellItem Create(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            // iterate over shellitem parsers and attempt to parse with each one, in order of priority.
            foreach (var parser in parsers)
                if (parser.CanParse(hive, keyWrapper, value, parent))
                {
                    var item = parser.Parse(hive, keyWrapper, value, parent);

                    if (item == null)
                        continue;

                    // replace the Place object created by the parser with an existing copy
                    // if the place has been observed before. This keeps place objects
                    // unique so they can be used as keys to search for shell items
                    if (hive != null && hive.Places.Contains(item.Place))
                    {
                        var place = hive.Places.First(place => place == item.Place);

                        item.GetType()
                            .GetField("fields", BindingFlags.Instance | BindingFlags.NonPublic)
                            .SetValue(item, place);
                    }
                    else if (hive != null)
                    {
                        hive.Places.Add(item.Place);
                    }

                    return item;
                }

            return null;
        }
    }
}
