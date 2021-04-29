using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Services
{
    /// <summary>
    /// An object that extracts shellbag timestamps and performs shell event generation
    /// </summary>
    public interface IShellEventManager
    {
        public event EventHandler ShellEventGenerateBegin;
        public event EventHandler ShellEventGenerateEnd;

        /// <summary>
        /// Generate shell events from a list of parsed shellbags
        /// </summary>
        /// <param name="shellItems">a list of parsed shellbags</param>
        /// <returns>a list of generated shell events</returns>
        public IEnumerable<IShellEvent> GenerateEvents(IEnumerable<IShellItem> shellItems);
    }
}
