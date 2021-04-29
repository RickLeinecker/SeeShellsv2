using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    /// <summary>
    /// An object that constructs ShellEvents from ShellItems and sequences of intermediate events
    /// </summary>
    public interface IShellEventFactory
    {
        /// <summary>
        /// Extract timestamps from the given ShellItem and use them to construct IIntermediateShellEvent instances.
        /// One IIntermediateShellEvent is created per timestamp
        /// </summary>
        /// <param name="item">The parsed shellbag to extract timestamps from</param>
        /// <returns>A list of intermediate events that were created from the shell item</returns>
        IEnumerable<IIntermediateShellEvent> CreateIntermediateEvents(IShellItem item);

        /// <summary>
        /// Analyze patterns in the sequence of IShellEvent objects to construct higher-level events.
        /// </summary>
        /// <param name="sequence">A sequence of events to analyze. These are usually IIntermediateShellEvent instances</param>
        /// <returns>A list of shell events that were detected using the given sequence</returns>
        IEnumerable<IShellEvent> CreateEvents(IEnumerable<IShellEvent> sequence);
    }
}
