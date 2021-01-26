using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using Unity.Resolution;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    public class ShellItemFactory : IAbstractFactory<IShellItem>
    {
        private readonly IUnityContainer container;

        public ShellItemFactory([Dependency] IUnityContainer container)
        {
            this.container = container;
        }

        public IShellItem Create(string subtype)
        {
            try
            {
                return container.Resolve<IShellItem>(subtype);
            }
            catch
            {
                return container.Resolve<IShellItem>();
            }
        }

        public IShellItem Create(string subtype, byte[] buf)
        {
            try
            {
                return container.Resolve<IShellItem>(subtype, new ParameterOverride("buf", buf));
            }
            catch
            {
                return container.Resolve<IShellItem>(new ParameterOverride("buf", buf));
            }
        }
    }
}
