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
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;

namespace SeeShellsV2.UI
{
    public class TimelineViewVM : ViewModel, ITimelineViewVM
    {
        public ISelected Selected { get; private set; }

        public ICollectionView ShellEvents => _shellEventsView.View;
        public ICollectionView FilteredShellEvents => _shellEvents.FilteredView;

        public string ColorProperty { get => _colorProperty; set { _colorProperty = value; NotifyPropertyChanged(); } }

        public DateTime? DateSelectionBegin { get => _selectionBegin; set { _selectionBegin = value; UpdateSelection(); } }
        public DateTime? DateSelectionEnd { get => _selectionEnd; set { _selectionEnd = value; UpdateSelection(); } }

        private void OnHistSelectionChanged(object sender, NotifyCollectionChangedEventArgs e) => UpdateSelection();

        private string _colorProperty = "TypeName";

        private DateTime? _selectionBegin = null;
        private DateTime? _selectionEnd = null;

        private IShellEventCollection _shellEvents;
        private readonly CollectionViewSource _shellEventsView = new CollectionViewSource();

        public TimelineViewVM([Dependency] ISelected selected, [Dependency] IShellEventCollection shellEvents)
        {
            Selected = selected;

            _shellEvents = shellEvents;
            _shellEventsView.Source = shellEvents;
            _shellEventsView.Filter += (o, a) => a.Accepted = FilteredShellEvents.Filter(a.Item) && CalendarHeatMapFilter(a.Item);

            _shellEvents.FilteredView.CollectionChanged += (_, a) => { if (a.Action == NotifyCollectionChangedAction.Reset) ShellEvents.Refresh(); };
        }

        private readonly SynchronizationContext context = SynchronizationContext.Current;
        private CancellationTokenSource tokenSource;
        private void UpdateSelection()
        {
            if (tokenSource != null)
                tokenSource.Cancel();

            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task.Run(() =>
            {
                Thread.Sleep(200);

                if (token.IsCancellationRequested)
                    return;

                context.Send((_) =>
                {
                    NotifyPropertyChanged(nameof(DateSelectionBegin));
                    NotifyPropertyChanged(nameof(DateSelectionEnd));
                    ShellEvents.Refresh();
                }, null);
            }, token);
        }

        public bool CalendarHeatMapFilter(object o)
        {
            return DateSelectionBegin == null || DateSelectionEnd == null ||
            (o is IShellEvent se && se.TimeStamp >= DateSelectionBegin && se.TimeStamp <= DateSelectionEnd);
        }
    }

    internal class RadioButtonSelector : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s1 && parameter is string s2)
                return s1 == s2;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }

    internal class EventTableSelector : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 3 && values[0] is object o && values[1] is string prop && values[2] is IEnumerable selected)
                return !selected.OfType<object>().Any() || selected.OfType<object>().Contains(o.GetDeepPropertyValue(prop));

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
