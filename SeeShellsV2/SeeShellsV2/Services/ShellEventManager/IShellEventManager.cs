using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Services
{
    public interface IShellEventManager
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ShellEventGenerateBegin;
        public event EventHandler ShellEventGenerateEnd;

        public IEnumerable<IShellEvent> GenerateEvents(IEnumerable<IShellItem> shellItems);
    }
}
