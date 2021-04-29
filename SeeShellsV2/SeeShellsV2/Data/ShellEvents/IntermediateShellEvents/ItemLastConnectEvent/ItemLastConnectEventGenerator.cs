using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ItemLastConnectEventGenerator : IIntermediateShellEventGenerator
    {
        public bool CanGenerate(IShellItem item)
        {
            return item is IConnectedTimestamp connected && connected.ConnectedDate != DateTime.MinValue;
        }

        public IEnumerable<IIntermediateShellEvent> Generate(IShellItem item)
        {
            if (!CanGenerate(item))
                yield break;

            IConnectedTimestamp connected = item as IConnectedTimestamp;

            yield return new ItemLastConnectEvent()
            {
                TypeName = "Item Last Connected",
                Description = string.Format("{0} Last Connected", connected.Place.Name),
                TimeStamp = connected.ConnectedDate,
                User = connected.RegistryHive.User,
                Place = connected.Place,
                Evidence = new List<IShellItem>() { connected }
            };
        }
    }
}
