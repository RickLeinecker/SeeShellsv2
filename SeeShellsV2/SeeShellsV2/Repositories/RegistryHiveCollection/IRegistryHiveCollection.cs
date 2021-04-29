using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    /// <summary>
    /// A collection of RegistryHive objects. Services and ViewModels can get a reference to this collection
    /// by declaring an appropriate constructor or property dependency.
    /// </summary>
    public interface IRegistryHiveCollection : IDataRepository<RegistryHive>
    {
    }
}
