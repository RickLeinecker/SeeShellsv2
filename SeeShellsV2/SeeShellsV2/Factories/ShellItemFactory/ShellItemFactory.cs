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

        public Type GetShellType(byte type)
        {
            return ShellItem.GetShellType(type);
        }

        public IShellItem Create(byte[] buf)
        {
            return ShellItem.FromByteArray(buf);
        }
    }
}
