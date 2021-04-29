using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class FIleDownloadEventGenerator : IShellEventGenerator
    {
        public int Priority => 1;

        public bool CanGenerate(IEnumerable<IShellEvent> sequence)
        {
            return GetMatches(sequence).Any();
        }

        public IEnumerable<IShellEvent> Generate(IEnumerable<IShellEvent> sequence)
        {
            var matches = GetMatches(sequence);

            foreach (var match in matches)
                match.Consumed = true;

            return matches.Select(e => new FileDownloadEvent()
            {
                TypeName = "File Download Event",
                Description = string.Format("{0} Downloaded", e.Place.Name.Split(Path.DirectorySeparatorChar).Last()),
                TimeStamp = e.TimeStamp,
                User = e.User,
                Place = e.Place,
                Evidence = e.Evidence
            });
        }

        private IEnumerable<ItemCreateEvent> GetMatches(IEnumerable<IShellEvent> sequence)
        {
            return sequence
                .OfType<ItemCreateEvent>()
                .Where(e => e.Place.PathName != null && e.Place.PathName.Contains("Downloads"));
        }
    }
}
