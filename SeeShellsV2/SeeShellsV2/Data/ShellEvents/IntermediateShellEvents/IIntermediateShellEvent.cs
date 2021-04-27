using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Represents a timestamp that was extracted from a shellbag
    /// </summary>
    public interface IIntermediateShellEvent : IShellEvent
    {
        /// <summary>
        /// true if this shell event has been converted
        /// to another type of event using an event generator
        /// </summary>
        public bool Consumed { get; set; }
    }
}
