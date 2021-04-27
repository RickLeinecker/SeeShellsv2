using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Used to identify shell items with a created timestamp
    /// </summary>
    public interface ICreationTimestamp : IShellItem
    {
        DateTime CreationDate { get; }
    }
}
