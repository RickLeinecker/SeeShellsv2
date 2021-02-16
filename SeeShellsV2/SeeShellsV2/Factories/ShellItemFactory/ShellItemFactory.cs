using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    public class ShellItemFactory : IShellItemFactory
    {

        public Type GetShellType(byte type, IShellItem parent = null)
        {
            return ShellItem.GetShellType(type, parent);
        }

        public IShellItem Create(byte[] buf, IShellItem parent = null)
        {
            return ShellItem.FromByteArray(buf, parent);
        }
    }
}
