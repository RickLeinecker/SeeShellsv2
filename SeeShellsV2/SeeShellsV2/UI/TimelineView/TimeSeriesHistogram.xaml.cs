using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace SeeShellsV2.UI
{
    /// <summary>
    /// Interaction logic for TimeSeriesHistogram.xaml
    /// </summary>
    public partial class TimeSeriesHistogram : UserControl
    {
        public IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProp) as IEnumerable;
            set => SetValue(ItemsSourceProp, value);
        }

        public string ColorProperty => "TypeName";
        public string DateTimeProperty { get; set; }

        public Color PlotAreaBorderColor { get; set; }
        public Color TextColor { get; set; }
        public Color TicklineColor { get; set; }

        public string YAxisTitle { get; set; }

        private ICollectionView Items { get; set; }
        private IList<TimeSeriesHistogramItem> Bins { get; set; }

        private readonly PlotModel _histPlotModel = new PlotModel();
        private readonly HistogramSeries _histSeries = new HistogramSeries();
        private readonly DateTimeAxis _dateAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
        private readonly LinearAxis _freqAxis = new LinearAxis { Position = AxisPosition.Left, IsZoomEnabled = false, IsPanEnabled = false };

        public TimeSeriesHistogram()
        {
            InitializeComponent();

            // _histPlotModel.Series.Add(_histSeries);
            _histPlotModel.Axes.Add(_dateAxis);
            _histPlotModel.Axes.Add(_freqAxis);

            _histPlotModel.MouseDown += _histPlotModel_MouseDown;

            HistogramPlot.Model = _histPlotModel;

            ResetHistSeries();
            UpdateAxes();
        }

        private void _histPlotModel_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton != OxyMouseButton.Left)
                return;

            if (_histPlotModel.LegendArea.Contains(e.Position))
            {
                int index = (int)((e.Position.Y - _histPlotModel.LegendArea.Top - _histPlotModel.LegendPadding) / _histPlotModel.LegendSymbolLength);

                try
                {
                    var hst = _histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.RenderInLegend).ElementAt(index);

                    if (hst.IsSelected())
                        hst.Unselect();
                    else
                        hst.Select();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }

                if (_histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.IsSelected()).Count() == 0)
                {
                    _histPlotModel.Series.OfType<HistogramSeries>().ForEach(s => s.FillColor = OxyColor.FromAColor(255, s.ActualFillColor));
                }
                else
                {
                    _histPlotModel.Series.OfType<HistogramSeries>().ForEach(s => s.FillColor = OxyColor.FromAColor((s.IsSelected() ? 255 : 32), s.ActualFillColor));
                }

                HistogramPlot.InvalidatePlot();
            }
        }

        private CancellationTokenSource _tokenSource = null;
        protected void OnItemsChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_tokenSource != null)
                _tokenSource.Cancel();

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                if (token.IsCancellationRequested)
                    return;

                Dispatcher.Invoke(() =>
                {
                    ResetHistSeries();
                    UpdateAxes();
                    HistogramPlot.InvalidatePlot();
                });
            }, token);
        }

        protected void UpdateAxes()
        {
            _histPlotModel.IsLegendVisible = true;
            _histPlotModel.LegendTextColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _histPlotModel.LegendTitleColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _histPlotModel.LegendPosition = LegendPosition.LeftTop;
            _histPlotModel.PlotAreaBorderColor = OxyColor.FromArgb(PlotAreaBorderColor.A, PlotAreaBorderColor.R, PlotAreaBorderColor.G, PlotAreaBorderColor.B);

            _dateAxis.TextColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _dateAxis.TicklineColor = OxyColor.FromArgb(TicklineColor.A, TicklineColor.R, TicklineColor.G, TicklineColor.B);

            _freqAxis.Title = YAxisTitle;

            _freqAxis.TitleColor = OxyColor.FromArgb(TextColor.A, TextColor.R, TextColor.G, TextColor.B);
            _freqAxis.TextColor = OxyColors.Transparent;
            _freqAxis.TicklineColor = OxyColor.FromArgb(TicklineColor.A, TicklineColor.R, TicklineColor.G, TicklineColor.B);
        }

        protected void ResetHistSeries()
        {
            if (Items == null || Items.OfType<object>().Count() < 2)
                return;

            IOrderedEnumerable<(DateTime date, object item)> items = Items
                .OfType<object>()
                .Select(x => ((DateTime)x.GetType().GetProperty(DateTimeProperty).GetValue(x, null), x))
                .OrderBy(x => x.Item1);

            BinningOptions options = new BinningOptions(
                BinningOutlierMode.CountOutliers,
                BinningIntervalType.InclusiveLowerBound,
                BinningExtremeValueMode.IncludeExtremeValues
            );

            var binBreaks = HistogramHelpers.CreateUniformBins(
                DateTimeAxis.ToDouble(items.First().date),
                DateTimeAxis.ToDouble(items.Last().date),
                100
            );

            var groups = items
                .GroupBy(x => x.item.GetType().GetProperty(ColorProperty).GetValue(x.item, null))
                .OrderByDescending(x => x.Count());

            _histPlotModel.Series.Clear();

            var bins = HistogramHelpers.Collect(
                items.Select(x => DateTimeAxis.ToDouble(x.date)),
                binBreaks,
                options
            ).Select(i => new TimeSeriesHistogramItem(i));

            foreach (var group in groups)
            {
                TimeSeriesHistogramSeries s = new TimeSeriesHistogramSeries();

                var dates = group
                .Select(x => x.date)
                .OrderBy(x => x);

                s.ItemsSource = HistogramHelpers.Collect(
                    dates.Select(x => DateTimeAxis.ToDouble(x)),
                    binBreaks,
                    options
                ).Select(i => new TimeSeriesHistogramItem(i))
                .Select(i => { i.Area = i.Area * dates.Count() / items.Count(); return i; });

                s.Title = group.Key.ToString();
                s.ToolTip = s.Title;

                _histPlotModel.Series.Add(s);
            }
        }

        protected void ResetHistBins()
        {
            if (Items == null || Items.OfType<object>().Count() < 2)
                return;

            var dates = Items
                .OfType<object>()
                .Select(x => (DateTime)x.GetType().GetProperty(DateTimeProperty).GetValue(x, null))
                .OrderBy(x => x);

            var binBreaks = HistogramHelpers.CreateUniformBins(DateTimeAxis.ToDouble(dates.First()), DateTimeAxis.ToDouble(dates.Last()), 100);
            BinningOptions options = new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.IncludeExtremeValues);
            Bins = HistogramHelpers.Collect(dates.Select(x => DateTimeAxis.ToDouble(x)), binBreaks, options).Select(i => new TimeSeriesHistogramItem(i)).ToList();

            _histSeries.Items.Clear();
            _histSeries.Items.AddRange(Bins);
        }

        public static readonly DependencyProperty ItemsSourceProp =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null, FrameworkPropertyMetadataOptions.AffectsRender,
                    (o, v) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            if (t.Items != null)
                                t.Items.CollectionChanged -= t.OnItemsChange;

                            t.Items = CollectionViewSource.GetDefaultView(v.NewValue);

                            if (t.Items != null)
                                t.Items.CollectionChanged += t.OnItemsChange;

                            t.HistogramPlot.InvalidatePlot();
                        }
                    }
                )
            );
    }

    public class TimeSeriesHistogramItem : HistogramItem
    {
        new public double Value => Count;

        public DateTime DateRangeStart => DateTimeAxis.ToDateTime(RangeStart);
        public DateTime DateRangeEnd => DateTimeAxis.ToDateTime(RangeEnd);
        public DateTime DateRangeCenter => DateRangeStart + ((DateRangeEnd - DateRangeStart) / 2);


        public TimeSeriesHistogramItem(HistogramItem item)
            : base(item.RangeStart, item.RangeEnd, item.Area, item.Count, item.Color)
        { }
    }

    public class TimeSeriesHistogramSeries : HistogramSeries
    {
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var result = base.GetNearestPoint(point, interpolate);

            if (result != null)
            {
                IList<(string, string)> trakerText = result.Text
                    .Split('\n')
                    .Select(s => s.Split(' '))
                    .Select(s => (s[0], s[1]))
                    .Where(s => s.Item1 != "Area:" && s.Item1 != "Value:")
                    .ToList();

                trakerText[0] = (trakerText[0].Item1, DateTimeAxis.ToDateTime(double.Parse(trakerText[0].Item2)).ToString());
                trakerText[1] = (trakerText[1].Item1, DateTimeAxis.ToDateTime(double.Parse(trakerText[1].Item2)).ToString());

                result.Text = trakerText
                    .Select(i => i.Item1 + ' ' + i.Item2)
                    .Aggregate(string.Empty, (string a, string i) => a + '\n' + i)
                    .Trim();
            }

            return result;
        }
    }
}
