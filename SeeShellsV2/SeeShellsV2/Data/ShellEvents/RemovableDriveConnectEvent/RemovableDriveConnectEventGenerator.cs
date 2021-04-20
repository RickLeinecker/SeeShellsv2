using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class RemovableDriveConnectEventGenerator : IShellEventGenerator
    {
        public int Priority => 1;

        public bool CanGenerate(IEnumerable<IShellEvent> sequence)
        {
            return sequence
                .OfType<ItemLastRegistryWriteEvent>()
                .Where(e => e.Place is RemovableDrive && !e.Consumed)
                .Any();
        }

        public IEnumerable<IShellEvent> Generate(IEnumerable<IShellEvent> sequence)
        {
            return sequence
                .OfType<ItemLastRegistryWriteEvent>()
                .Where(e => e.Place is RemovableDrive && !e.Consumed)
                .Select(e => new RemovableDriveConnectEvent
                {
                    TypeName = "Removable Storage Device Connect",
                    Description = string.Format("{0} Connected", e.Place.Name),
                    TimeStamp = e.TimeStamp,
                    User = e.User,
                    Place = e.Place,
                    Evidence = e.Evidence
                });
        }
    }
}
