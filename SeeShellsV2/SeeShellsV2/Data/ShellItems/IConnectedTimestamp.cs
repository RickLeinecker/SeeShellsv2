using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Used to identify shell items with a last connected timestamp
    /// </summary>
    public interface IConnectedTimestamp : IShellItem
    {
        DateTime ConnectedDate { get; }
    }
}
