using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    public interface IShellEventFactory
    {
        IEnumerable<IIntermediateShellEvent> CreateIntermediateEvents(IShellItem item);

        IEnumerable<IShellEvent> CreateEvents(IEnumerable<IShellEvent> sequence);
    }
}
