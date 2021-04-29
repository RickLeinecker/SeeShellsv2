using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ItemLastAccessEventGenerator : IIntermediateShellEventGenerator
    {
        public bool CanGenerate(IShellItem item)
        {
            return item is IAccessedTimestamp access && access.AccessedDate != DateTime.MinValue;
        }

        public IEnumerable<IIntermediateShellEvent> Generate(IShellItem item)
        {
            if (!CanGenerate(item))
                yield break;

            IAccessedTimestamp access = item as IAccessedTimestamp;

            yield return new ItemLastAccessEvent()
            {
                TypeName = "Item Last Access",
                Description = string.Format("{0} Last Accessed", access.Place.Name),
                TimeStamp = access.AccessedDate,
                User = access.RegistryHive.User,
                Place = access.Place,
                Evidence = new List<IShellItem>() { access }
            };
        }
    }
}
