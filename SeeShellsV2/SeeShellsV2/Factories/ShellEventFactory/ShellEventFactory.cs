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
            // construct event generators
            CreateFullEventGenerators(container);
            CreateIntermediateEventGenerators(container);
        }

        public IEnumerable<IIntermediateShellEvent> CreateIntermediateEvents(IShellItem item)
        {
            List<IIntermediateShellEvent> shellEvents = new List<IIntermediateShellEvent>();

            // try to generate events for every type, in order of priority
            foreach (var generator in iGenerators)
                if (generator.CanGenerate(item))
                    shellEvents.AddRange(generator.Generate(item));

            return shellEvents;
        }

        public IEnumerable<IShellEvent> CreateEvents(IEnumerable<IShellEvent> sequence)
        {
            List<IShellEvent> shellEvents = new List<IShellEvent>();

            // try to generate events for every type, in order of priority
            foreach (var generator in fGenerators)
                if (generator.CanGenerate(sequence))
                    shellEvents.AddRange(generator.Generate(sequence));

            return shellEvents;
        }

        private void CreateFullEventGenerators(IUnityContainer container)
        {
            // get all implementations of IShellEventGenerator from the assembly,
            // construct an instance of each one, and sort according to priority.
            // generators will be used by the factory to construct events
            fGenerators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IShellEventGenerator).IsAssignableFrom(p))
                .Where(q => q.IsClass)
                .Select(r => (IShellEventGenerator) container.Resolve(r))
                .OrderByDescending(g => g.Priority)
                .ToList();
        }

        private void CreateIntermediateEventGenerators(IUnityContainer container)
        {
            // get all implementations of IIntermediateShellEventGenerator from the assembly
            // and construct an instance of each one.
            // generators will be used by the factory to construct intermediate events
            iGenerators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IIntermediateShellEventGenerator).IsAssignableFrom(p))
                .Where(q => q.IsClass)
                .Select(r => (IIntermediateShellEventGenerator)container.Resolve(r))
                .ToList();
        }
    }
}
