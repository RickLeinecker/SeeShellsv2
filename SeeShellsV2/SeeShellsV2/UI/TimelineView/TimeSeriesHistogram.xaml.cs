using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
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
        public IEnumerable ItemsSource { get => GetValue(ItemsSourceProp) as IEnumerable; set => SetValue(ItemsSourceProp, value); }

        public DateTime? MinimumDate { get => GetValue(MinimumDateProp) as DateTime?; set => SetValue(MinimumDateProp, value); }
        public DateTime? MaximumDate { get => GetValue(MaximumDateProp) as DateTime?; set => SetValue(MaximumDateProp, value); }

        public string ColorProperty { get => GetValue(ColorPropertyProp) as string; set => SetValue(ColorPropertyProp, value); }
        public string DateTimeProperty { get => GetValue(DateTimePropertyProp) as string; set => SetValue(DateTimePropertyProp, value); }

        public Color PlotAreaBorderColor { get => (Color) GetValue(PlotAreaBorderColorProp); set => SetValue(PlotAreaBorderColorProp, value); }
        public Color TextColor { get => (Color) GetValue(TextColorProp); set => SetValue(TextColorProp, value); }
        public Color TicklineColor { get => (Color)GetValue(TicklineColorProp); set => SetValue(TicklineColorProp, value); }

        public string YAxisTitle { get; set; }

        public ICollectionView Selected { get => CollectionViewSource.GetDefaultView(_selected); }

        private ICollectionView Items { get; set; }

        private readonly PlotModel _histPlotModel = new PlotModel();
        private readonly DateTimeAxis _dateAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
        private readonly LinearAxis _freqAxis = new LinearAxis { Position = AxisPosition.Left, IsZoomEnabled = false, IsPanEnabled = false };

        private readonly ObservableCollection<object> _selected = new ObservableCollection<object>();

        public TimeSeriesHistogram()
        {
            InitializeComponent();

            _histPlotModel.Axes.Add(_dateAxis);
            _histPlotModel.Axes.Add(_freqAxis);

            _histPlotModel.MouseDown += _histPlotModel_MouseDown;
            _histPlotModel.MouseMove += _histPlotModel_MouseMove;

            HistogramPlot.Model = _histPlotModel;

            ResetHistSeries();
            UpdateAxes();
        }

        private void _histPlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var results = _histPlotModel.HitTest(new HitTestArguments(e.Position, 10));

                var result = results
                    .Where(r => r.Element is Series)
                    .Select(r => (r, r.Element as Series))
                    .OrderBy(r => !r.Item2.IsSelected())
                    .ThenBy(r => (r.Item2.Tag as (object, int)?)?.Item2 ?? 0)
                    .Select(r => r.Item2.GetNearestPoint(e.Position, true))
                    .FirstOrDefault();

                if (result != null)
                {
                    HistogramPlot.ShowTracker(result);
                    e.Handled = true;
                    return;
                }
            }

            HistogramPlot.HideTracker();
        }

        private void _histPlotModel_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton != OxyMouseButton.Left)
                return;

            if (_histPlotModel.LegendArea.Contains(e.Position))
            {
                int index = (int)((e.Position.Y - _histPlotModel.LegendArea.Top - _histPlotModel.LegendPadding) / (_histPlotModel.LegendSymbolLength + 1));

                try
                {
                    var hst = _histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.RenderInLegend).ElementAt(index);

                    if (hst.IsSelected())
                    {
                        hst.Unselect();
                        _selected.Remove((hst.Tag as (object, int)?)?.Item1);
                    }
                    else
                    {
                        hst.Select();
                        _selected.Add((hst.Tag as (object, int)?)?.Item1);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }

                UpdateColors();
                HistogramPlot.InvalidatePlot();
            }
            else
            {
                _histPlotModel_MouseMove(sender, e);
            }
        }

        protected void OnItemsChange(object sender, NotifyCollectionChangedEventArgs args) => Update();

        private CancellationTokenSource _tokenSource = null;
        protected void Update()
        {
            if (_tokenSource != null)
                _tokenSource.Cancel();

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            Task.Run(() =>
            {
                Thread.Sleep(200);

                if (token.IsCancellationRequested)
                    return;

                Dispatcher.Invoke(() =>
                {
                    ResetHistSeries();
                    UpdateAxes();
                    UpdateColors();
                    HistogramPlot.InvalidatePlot();
                });
            }, token);
        }

        protected void UpdateColors()
        {
            HistogramPlot.InvalidatePlot();

            if (!_histPlotModel.Series.OfType<HistogramSeries>().Where(s => s.IsSelected()).Any())
                _histPlotModel.Series.OfType<HistogramSeries>().ForEach(s => s.FillColor = OxyColor.FromAColor((byte)255, s.ActualFillColor));
            else
                _histPlotModel.Series.OfType<HistogramSeries>().ForEach(s => s.FillColor = OxyColor.FromAColor((byte)(s.IsSelected() ? 255 : 32), s.ActualFillColor));
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
            _histPlotModel.Series.Clear();

            if (Items == null || !Items.OfType<object>().Any())
                return;

            IOrderedEnumerable<(DateTime date, object item)> items = Items
                .OfType<object>()
                .Select(x => ((DateTime)x.GetDeepPropertyValue(DateTimeProperty), x))
                .OrderBy(x => x.Item1);

            BinningOptions options = new BinningOptions(
                BinningOutlierMode.CountOutliers,
                BinningIntervalType.InclusiveLowerBound,
                BinningExtremeValueMode.IncludeExtremeValues
            );

            DateTime begin = MinimumDate ?? items.First().date;
            DateTime end = MaximumDate ?? items.Last().date;

            if (end == begin)
                end = end.AddMinutes(15);

            var binBreaks = HistogramHelpers.CreateUniformBins(
                DateTimeAxis.ToDouble(begin),
                DateTimeAxis.ToDouble(end),
                50
            );

            IOrderedEnumerable<IGrouping<object, (DateTime date, object item)>> groups;
            if (ColorProperty == null)
                groups = items.GroupBy(x => (object) null).OrderByDescending(x => x.Count());
            else
                groups = items
                    .GroupBy(x => x.item.GetDeepPropertyValue(ColorProperty))
                    .OrderByDescending(x => x.Count());

            foreach (var group in groups)
            {
                HistogramSeries s = new HistogramSeries();

                var dates = group
                .Select(x => x.date)
                .OrderBy(x => x);

                s.ItemsSource = HistogramHelpers.Collect(
                    dates.Select(x => DateTimeAxis.ToDouble(x)),
                    binBreaks,
                    options
                );

                s.ItemsSource
                    .OfType<HistogramItem>()
                    .ForEach(i => { i.Area = i.Area * dates.Count() / items.Count(); });

                s.Title = group.Key?.ToString() ?? string.Empty;
                s.ToolTip = s.Title;
                s.Tag = (group.Key, dates.Count());

                if (_selected.Contains(group.Key))
                    s.Select();

                _histPlotModel.Series.Add(s);
            }
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

                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty MinimumDateProp =
            DependencyProperty.Register(
                nameof(MinimumDate),
                typeof(DateTime?),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            if (a.NewValue is DateTime d)
                            {
                                t._dateAxis.Reset();
                                t._dateAxis.Minimum = DateTimeAxis.ToDouble(d);
                            }
                            else
                            {
                                t._dateAxis.Minimum = double.NaN;
                            }

                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty MaximumDateProp =
            DependencyProperty.Register(
                nameof(MaximumDate),
                typeof(DateTime?),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            if (a.NewValue is DateTime d)
                            {
                                t._dateAxis.Maximum = DateTimeAxis.ToDouble(d);
                            }
                            else
                            {
                                t._dateAxis.Maximum = double.NaN;
                            }

                            t._dateAxis.Reset();
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty DateTimePropertyProp =
            DependencyProperty.Register(
                nameof(DateTimeProperty),
                typeof(string),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty ColorPropertyProp =
            DependencyProperty.Register(
                nameof(ColorProperty),
                typeof(string),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    null,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t._selected.Clear();
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty TextColorProp =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty PlotAreaBorderColorProp =
            DependencyProperty.Register(
                nameof(PlotAreaBorderColor),
                typeof(Color),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );

        public static readonly DependencyProperty TicklineColorProp =
            DependencyProperty.Register(
                nameof(TicklineColor),
                typeof(Color),
                typeof(TimeSeriesHistogram),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    (o, a) =>
                    {
                        if (o is TimeSeriesHistogram t)
                        {
                            t.Update();
                        }
                    }
                )
            );
    }

    internal class TimeSeriesHistogramTrackerTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is TrackerHitResult result && values[1] is TimeSeriesHistogram t)
            {
                IList<(string, string)> trakerText = result.Text
                    .Split('\n')
                    .Select(s => s.Split(' '))
                    .Select(s => (s[0], s[1]))
                    .Where(s => s.Item1 != "Area:" && s.Item1 != "Value:")
                    .ToList();

                trakerText[0] = (trakerText[0].Item1, DateTimeAxis.ToDateTime(double.Parse(trakerText[0].Item2)).ToString());
                trakerText[1] = (trakerText[1].Item1, DateTimeAxis.ToDateTime(double.Parse(trakerText[1].Item2)).ToString());

                return result.Series.Title + "\n" + trakerText
                    .Select(i => i.Item1 + ' ' + i.Item2)
                    .Aggregate(string.Empty, (string a, string i) => a + '\n' + i)
                    .Trim();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
