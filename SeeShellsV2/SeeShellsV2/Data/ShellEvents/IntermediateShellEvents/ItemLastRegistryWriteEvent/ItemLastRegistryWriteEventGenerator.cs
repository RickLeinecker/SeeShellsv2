using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ItemLastRegistryWriteEventGenerator : IIntermediateShellEventGenerator
    {
        public bool CanGenerate(IShellItem item)
        {
            return !(item is UnknownShellItem) && item.LastRegistryWriteDate != DateTime.MinValue;
        }

        public IEnumerable<IIntermediateShellEvent> Generate(IShellItem item)
        {
            if (!CanGenerate(item))
                yield break;

            yield return new ItemLastRegistryWriteEvent()
            {
                TypeName = "Item Last Registry Write",
                Description = string.Format("{0} Last Registry Write", item.Place.Name),
                TimeStamp = item.LastRegistryWriteDate,
                User = item.RegistryHive.User,
                Place = item.Place,
                Evidence = new List<IShellItem>() { item }
            };
        }
    }
}
