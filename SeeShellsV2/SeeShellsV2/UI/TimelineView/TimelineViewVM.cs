using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;

namespace SeeShellsV2.UI
{
    public class TimelineViewVM : ViewModel, ITimelineViewVM
    {
        [Dependency]
        public ISelected Selected { get; set; }

        [Dependency]
        public IShellEventCollection ShellEvents { get; set; }

        [Dependency]
        public TemporaryShellEventGeneratorDontUseMe EventGenerator { get; set; }

        public TimelineViewVM() { }

        public void GenerateRandomShellEvents()
        {
            EventGenerator.Generate();
        }
    }
}
