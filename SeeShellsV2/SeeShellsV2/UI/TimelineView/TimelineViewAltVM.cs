using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Unity;
using OxyPlot;
using OxyPlot.Series;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Utilities;
using System.Globalization;
using OxyPlot.Axes;

namespace SeeShellsV2.UI
{
    public class TimelineViewAltVM : ViewModel, ITimelineViewAltVM
    {
        public ISelected Selected { get; private set; }

        public ICollectionView ShellEvents => _shellEventsView.View;
        public ICollectionView FilteredShellEvents => _shellEvents.FilteredView;

        public DateTime? SelectionBegin { get => _selectionBegin; set { _selectionBegin = value; ShellEvents.Refresh(); } }
        public DateTime? SelectionEnd { get => _selectionEnd; set { _selectionEnd = value; ShellEvents.Refresh(); } }

        private DateTime? _selectionBegin = null;
        private DateTime? _selectionEnd = null;


        private IShellEventCollection _shellEvents;
        private readonly CollectionViewSource _shellEventsView = new CollectionViewSource();

        public TimelineViewAltVM([Dependency] ISelected selected, [Dependency] IShellEventCollection shellEvents)
        {
            Selected = selected;

            _shellEvents = shellEvents;
            _shellEventsView.Source = shellEvents;
            _shellEventsView.Filter += (o, a) => a.Accepted = FilteredShellEvents.Filter(a.Item) && CalendarHeatMapFilter(a.Item);

            _shellEvents.FilteredView.CollectionChanged += (_, a) => { if (a.Action == NotifyCollectionChangedAction.Reset) ShellEvents.Refresh(); };
        }

        public bool CalendarHeatMapFilter(object o)
        {
                return SelectionBegin == null || SelectionEnd == null ||
                (o is IShellEvent se && se.TimeStamp >= SelectionBegin && se.TimeStamp <= SelectionEnd);
        }
    }
}
