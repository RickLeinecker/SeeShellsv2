using System;
using System.Collections.Generic;
using System.Linq;

using Unity;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    public class ShellEventFactory : IShellEventFactory
    {
        private IList<IIntermediateShellEventGenerator> iGenerators;
        private IList<IShellEventGenerator> fGenerators;

        public ShellEventFactory([Dependency] IUnityContainer container)
        {
            CreateFullEventGenerators(container);
            CreateIntermediateEventGenerators(container);
        }

        public IEnumerable<IIntermediateShellEvent> CreateIntermediateEvents(IShellItem item)
        {
            List<IIntermediateShellEvent> shellEvents = new List<IIntermediateShellEvent>();

            foreach (var generator in iGenerators)
                if (generator.CanGenerate(item))
                    shellEvents.AddRange(generator.Generate(item));

            return shellEvents;
        }

        public IEnumerable<IShellEvent> CreateEvents(IEnumerable<IShellEvent> sequence)
        {
            List<IShellEvent> shellEvents = new List<IShellEvent>();

            foreach (var generator in fGenerators)
                if (generator.CanGenerate(sequence))
                    shellEvents.AddRange(generator.Generate(sequence));

            return shellEvents;
        }

        private void CreateFullEventGenerators(IUnityContainer container)
        {
            fGenerators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IShellEventGenerator).IsAssignableFrom(p))
                .Where(q => q.IsClass)
                .Select(r => (IShellEventGenerator) container.Resolve(r))
                .OrderBy(g => g.Priority)
                .ToList();
        }

        private void CreateIntermediateEventGenerators(IUnityContainer container)
        {
            iGenerators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IIntermediateShellEventGenerator).IsAssignableFrom(p))
                .Where(q => q.IsClass)
                .Select(r => (IIntermediateShellEventGenerator)container.Resolve(r))
                .ToList();
        }
    }
}
