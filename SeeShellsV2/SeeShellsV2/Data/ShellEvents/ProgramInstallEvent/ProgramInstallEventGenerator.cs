using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ProgramInstallEventGenerator : IShellEventGenerator
    {
        public int Priority => 1;

        public bool CanGenerate(IEnumerable<IShellEvent> sequence)
        {
            return GetMatches(sequence).Any();
        }

        public IEnumerable<IShellEvent> Generate(IEnumerable<IShellEvent> sequence)
        {
            var matches = GetMatches(sequence);

            if (!matches.Any())
                return null;

            foreach (var match in matches)
                match.Consumed = true;

            return matches.Select(e => new ProgramInstallEvent()
            {
                TypeName = "Program Installation Event",
                Description = string.Format("{0} Installed", e.Place.Name),
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
                .Where(e =>
                {
                    string parent_dir = e.Place.PathName?.Split(Path.DirectorySeparatorChar).Last() ?? string.Empty;
                    return parent_dir == "Programs" || parent_dir == "Program Files" || parent_dir == "Program Files (x86)";
                });
        }
    }
}
