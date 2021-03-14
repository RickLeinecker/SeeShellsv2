using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ItemCreateEventGenerator : IIntermediateShellEventGenerator
    {
        public bool CanGenerate(IShellItem item)
        {
            return item is ICreationTimestamp create && create.CreationDate != DateTime.MinValue;
        }

        public IEnumerable<IIntermediateShellEvent> Generate(IShellItem item)
        {
            if (!CanGenerate(item))
                yield break;

            ICreationTimestamp create = item as ICreationTimestamp;

            yield return new ItemCreateEvent()
            {
                TypeName = "Item Creation",
                Description = string.Format("{0} Created", create.Place.Name),
                TimeStamp = create.CreationDate,
                User = create.RegistryHive.User,
                Place = create.Place,
                Evidence = new List<IShellItem>() { create }
            };
        }
    }
}
