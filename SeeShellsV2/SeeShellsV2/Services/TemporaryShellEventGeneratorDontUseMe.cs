using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.Services
{
    /// <summary>
    /// A service that generates dummy shell events for UI development
    /// </summary>
    public class TemporaryShellEventGeneratorDontUseMe
    {
        private IShellEventCollection collection;

        public TemporaryShellEventGeneratorDontUseMe([Dependency] IShellEventCollection collection)
        {
            this.collection = collection;
        }
        
        /// <summary>
        /// Generate between 200 and 2000 random shell events on a background thread
        /// </summary>
        /// <param name="seed">value used to seed the psuedo-random number generator</param>
        /// <returns>shell event generation task</returns>
        public Task Generate(int seed = 1618)
        {
            SynchronizationContext syncher = SynchronizationContext.Current;

            return Task.Run(() =>
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random rand = new Random(seed);

                for (int i = 0; i < rand.Next(200, 2001); i++)
                {
                    IShellEvent e = new ShellEvent()
                    {
                        TypeName = new string(Enumerable.Repeat(chars, rand.Next(1, 10))
                            .Select(s => s[rand.Next(s.Length)]).ToArray()),
                        Description = new string(Enumerable.Repeat(chars, rand.Next(1, 10))
                            .Select(s => s[rand.Next(s.Length)]).ToArray()),
                        TimeStamp = new DateTime(rand.Next(637134336, 637450560) * 1000000000L),
                        User = new User()
                        {
                            Name = new string(Enumerable.Repeat(chars, rand.Next(1, 10))
                            .Select(s => s[rand.Next(s.Length)]).ToArray())
                        },
                        Place = new Place()
                        {
                            Name = new string(Enumerable.Repeat(chars, rand.Next(1, 10))
                            .Select(s => s[rand.Next(s.Length)]).ToArray())
                        },
                        Evidence = new List<ShellItem>() { new ShellItem() { Description = "Fake Shell Item" } }
                    };

                    syncher.Post(delegate { collection.Add(e); }, null);
                }
            });
        }
    }
}
