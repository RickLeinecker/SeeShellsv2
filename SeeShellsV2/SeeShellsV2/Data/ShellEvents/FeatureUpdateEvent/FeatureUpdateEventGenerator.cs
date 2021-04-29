using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class FeatureUpdateEventGenerator : IShellEventGenerator
    {
        public int Priority => 2;

        public bool CanGenerate(IEnumerable<IShellEvent> sequence)
        {
            // check if the sequence of events contains any large
            // subsequences of ItemLastRegistryWriteEvents with the
            // same timestamp
            return GetSubsequences(sequence).Any();
        }

        public IEnumerable<IShellEvent> Generate(IEnumerable<IShellEvent> sequence)
        {
            if (!CanGenerate(sequence))
                return null;

            var subsequences = GetSubsequences(sequence);

            foreach (var subsequence in subsequences)
                foreach (var e in subsequence)
                        e.Consumed = true;

            return subsequences.Select(s => new FeatureUpdateEvent()
                {
                    TypeName = "Feature Update Event",
                    Description = "Probable Windows Feature Update",
                    TimeStamp = s.First().TimeStamp,
                    User = s.First().User,
                    Place = new Place() { Name = "System" },
                    Evidence = s
                    .Select(e=>e.Evidence)
                    .Aggregate(new List<IShellItem>(), (accum, e) => { accum.AddRange(e); return accum; })
                });
        }

        private IEnumerable<IEnumerable<ItemLastRegistryWriteEvent>> GetSubsequences(IEnumerable<IShellEvent> sequence)
        {
            // Return large, contiguous subseqences of last registry write events
            // with the same timestamp (probably windows feature update)
            return sequence
                .OfType<ItemLastRegistryWriteEvent>()
                .GroupWhile((p, n) => p.TimeStamp.Subtract(n.TimeStamp).Duration().Minutes <= 1)
                .Where(s => s.Count() >= sequence.OfType<ItemLastRegistryWriteEvent>().Count() / 10);
        }
    }
}
