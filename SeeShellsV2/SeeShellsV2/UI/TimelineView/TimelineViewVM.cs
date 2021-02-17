using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using SeeShellsV2.Data;

namespace SeeShellsV2.UI
{
    public class TimelineViewVM : ViewModel, ITimelineViewVM
    {
        private IShellItem item = new ShellItem();

        public IShellItem SelectedShell
        {
            get => item;
            set
            {
                item = value;
                NotifyPropertyChanged();
            }
        }
    }
}
