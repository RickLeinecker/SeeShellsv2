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
using OxyPlot.Wpf;

namespace SeeShellsV2.UI
{
    public partial class CalendarHeatMap : UserControl
    {
        public int Year { get => (int)GetValue(YearProp); set => SetValue(YearProp, value); }

        public DateTime? SelectionBegin { get => GetValue(SelectionBeginProp) as DateTime?; set => SetValue(SelectionBeginProp, value); }
        public DateTime? SelectionEnd { get => GetValue(SelectionEndProp) as DateTime?; set => SetValue(SelectionEndProp, value); }

        public IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProp) as IEnumerable;
            set => SetValue(ItemsSourceProp, value);
        }
        public string DateTimeProperty { get; set; }

        public string ColorAxisTitle { get; set; }

        public Orientation Orientation { get => (Orientation)GetValue(OrientationProp); set => SetValue(OrientationProp, value); }

        public CalendarHeatMap()
        {
            InitializeComponent();
            (HeatMapSeries.InternalSeries as OxyPlot.Series.HeatMapSeries).Interpolate = false;
            // SizeChanged += OnSizeChanged;
        }

        public void ClearSelected()
        {
            _last_selected = null;
            SelectionBegin = SelectionEnd = null; //_selected.Clear();
        }

        private ICollectionView Items { get; set; }

        private void Left_Button_Click(object sender, RoutedEventArgs e) => SetCurrentValue(YearProp, Year - 1);

        private void Right_Button_Click(object sender, RoutedEventArgs e) => SetCurrentValue(YearProp, Year + 1);

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearSelected();
        }

        private DateTime? _last_selected = null;
        //private Point? _last_selected = null;
        //private readonly SortedSet<Point> _selected = new SortedSet<Point>(Comparer<Point>.Create((Point a, Point b) => (a.X, a.Y).CompareTo((b.X, b.Y))));
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point p;
            DateTime date;
            double i, j;

            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                _last_selected = null;
                return;
            }

            OxyPlot.OxyMouseEventArgs args = ConverterExtensions.ToMouseEventArgs(e, sender as IInputElement);

            if (Orientation == Orientation.Vertical)
            {
                i = Math.Ceiling(DayAxis.InternalAxis.InverseTransform(args.Position.X) - 0.5);
                j = Math.Ceiling(WeekAxis.InternalAxis.InverseTransform(args.Position.Y) - 0.5);
                date = new DateTime(Year, 1, 1).AddDays((52 - j) * 7 + i - (int)new DateTime(Year, 1, 1).DayOfWeek);
                p = new Point(i, j);
            }
            else
            {
                i = Math.Ceiling(WeekAxis.InternalAxis.InverseTransform(args.Position.X) - 0.5);
                j = Math.Ceiling(DayAxis.InternalAxis.InverseTransform(args.Position.Y) - 0.5);
                date = new DateTime(Year, 1, 1).AddDays(i * 7 + (6 - j) - (int)new DateTime(Year, 1, 1).DayOfWeek);
                p = new Point(i, j);
            }

            if (date.Year != Year)
                return;

            if (_last_selected != null && date == _last_selected)
                return;

            _last_selected = date;

            if (SelectionBegin == null || SelectionEnd == null)
            {
                SelectionBegin = date; SelectionEnd = date.AddDays(1);
            }
            else if (_last_selected == SelectionEnd || date > SelectionBegin)
                SelectionEnd = date.AddDays(1);
            else if (_last_selected == SelectionBegin || date < SelectionEnd)
                SelectionBegin = date;

            //if (_selected.Contains(p))
            //    _selected.Remove(p);
            //else
            //    _selected.Add(p);
        }

        private readonly double _aspectRatio = 203.42857142857144 / 723.3733333333333;
        protected void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            double ar = (args.NewSize.Width < args.NewSize.Height) ? _aspectRatio : 1 / _aspectRatio;

            if (args.HeightChanged)
                Width = args.NewSize.Height * ar;
            else if (args.WidthChanged)
                Height = args.NewSize.Width / ar;
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
                Thread.Sleep(200);

                if (token.IsCancellationRequested)
                    return;

                Dispatcher.Invoke(() =>
                {
                    ResetHeatMap();
                    HeatMapPlot.InvalidatePlot();
                });
            }, token);
        }

        private readonly string[] weekdays = new string[] { "S", "M", "T", "W", "T", "F", "S" };
        private readonly string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        protected void UpdateGrid()
        {
            HeatMapPlot.Annotations.Clear();

            for (int i = 0; i < 6; i++)
                HeatMapPlot.Annotations.Add(new LineAnnotation
                {
                    X = (Orientation == Orientation.Vertical) ? 0.5 + i : 0,
                    Y = (Orientation == Orientation.Vertical) ? 0 : 0.5 + i,
                    Type = (Orientation == Orientation.Vertical) ?
                        OxyPlot.Annotations.LineAnnotationType.Vertical : OxyPlot.Annotations.LineAnnotationType.Horizontal,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    Color = HeatMapPlot.PlotAreaBorderColor
                });

            for (int i = 0; i < 53; i++)
                HeatMapPlot.Annotations.Add(new LineAnnotation
                {
                    X = (Orientation == Orientation.Horizontal) ? 0.5 + i : 0,
                    Y = (Orientation == Orientation.Horizontal) ? 0 : 0.5 + i,
                    Type = (Orientation == Orientation.Horizontal) ?
                        OxyPlot.Annotations.LineAnnotationType.Vertical : OxyPlot.Annotations.LineAnnotationType.Horizontal,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    Color = HeatMapPlot.PlotAreaBorderColor
                });

            DateTime begin = new DateTime(Year, 1, 1);
            for (int i = 1; i <= 12; i++)
            {
                int p0 = new DateTime(Year, i, 1).DayOfYear;
                int p1 = p0 + DateTime.DaysInMonth(Year, i) - 1;

                if (Orientation == Orientation.Vertical)
                {
                    HeatMapPlot.Annotations.Add(new PolygonAnnotation
                    {
                        Points = new List<OxyPlot.DataPoint>
                    {
                        new OxyPlot.DataPoint(-0.5, 52 - ((int)begin.DayOfWeek + p1 - 1 + 7) / 7 + 0.5),
                        new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5, 52 - ((int)begin.DayOfWeek + p1 - 1) / 7 - 0.5),
                        new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5, 52 - ((int)begin.DayOfWeek + p1 - 1) / 7 + 0.5),
                        new OxyPlot.DataPoint(6.5, 52 - ((int)begin.DayOfWeek + p1 - 1) / 7 + 0.5),

                        new OxyPlot.DataPoint(6.5, 52 - ((int)begin.DayOfWeek + p0 - 1 - 7) / 7 + ((i == 1) ?  0.5 : -0.5)),
                        new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5, 52 - ((int)begin.DayOfWeek + p0 - 1) / 7 + 0.5),
                        new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5, 52 - ((int)begin.DayOfWeek + p0 - 1) / 7 - 0.5),
                        new OxyPlot.DataPoint(-0.5, 52 - ((int)begin.DayOfWeek + p0 - 1 + 7) / 7 + 0.5),
                    },
                        LineStyle = OxyPlot.LineStyle.Solid,
                        Stroke = MonthAxis.TextColor,
                        Fill = Colors.Transparent,
                        StrokeThickness = 1
                    });
                }
                else
                {
                    // TODO
                }
            }

            if (SelectionBegin is DateTime start && SelectionEnd is DateTime end)
            {
                int p0 = start.DayOfYear;
                int p1 = end.DayOfYear - 1;

                if (Orientation == Orientation.Vertical)
                {
                    HeatMapPlot.Annotations.Add(new PolygonAnnotation
                    {
                        Points = new List<OxyPlot.DataPoint>
                        {
                            new OxyPlot.DataPoint(-0.5, 52 - ((int)begin.DayOfWeek + p1 - 1 + 7) / 7 + 0.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5, 52 - ((int)begin.DayOfWeek + p1 - 1) / 7 - 0.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5, 52 - ((int)begin.DayOfWeek + p1 - 1) / 7 + 0.5),
                            new OxyPlot.DataPoint(6.5, 52 - ((int)begin.DayOfWeek + p1 - 1) / 7 + 0.5),

                            new OxyPlot.DataPoint(6.5, 52 - ((int)begin.DayOfWeek + p0 - 1 - 7) / 7 + (((p0 + (int)begin.DayOfWeek) <= 6) ? 0.5 : -0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5, 52 - ((int)begin.DayOfWeek + p0 - 1) / 7 + 0.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5, 52 - ((int)begin.DayOfWeek + p0 - 1) / 7 - 0.5),
                            new OxyPlot.DataPoint(-0.5, 52 - ((int)begin.DayOfWeek + p0 - 1 + 7) / 7 + 0.5),
                        }
                    });
                }
                else
                {
                    // TODO
                }
            }

            //foreach (Point p in _selected)
            //{
            //    HeatMapPlot.Annotations.Add(new RectangleAnnotation
            //    {
            //        MinimumX = p.X-0.5,
            //        MaximumX = p.X+0.5,
            //        MinimumY = p.Y-0.5,
            //        MaximumY = p.Y+0.5
            //    });
            //}
        }

        protected void UpdateAxes()
        {
            HeatMapTitle.Text = Year.ToString();

            ColorAxis.Title = ColorAxisTitle;

            DayAxis.Position = (Orientation == Orientation.Horizontal) ?
                OxyPlot.Axes.AxisPosition.Left : OxyPlot.Axes.AxisPosition.Bottom;

            DayAxis.ItemsSource = (Orientation == Orientation.Horizontal) ?
                weekdays.Reverse() : weekdays;

            WeekAxis.Position = (Orientation == Orientation.Horizontal) ?
                        OxyPlot.Axes.AxisPosition.Top : OxyPlot.Axes.AxisPosition.Right;

            WeekAxis.Minimum = -0.5;
            WeekAxis.Maximum = 52.5;

            MonthAxis.Position = (Orientation == Orientation.Horizontal) ?
                OxyPlot.Axes.AxisPosition.Bottom : OxyPlot.Axes.AxisPosition.Left;

            MonthAxis.ItemsSource = (Orientation == Orientation.Horizontal) ?
                months : months.Reverse();

            HeatMapSeries.X0 = 0;
            HeatMapSeries.X1 = (Orientation == Orientation.Horizontal) ? 52 : 6;
            HeatMapSeries.Y0 = 0;
            HeatMapSeries.Y1 = (Orientation == Orientation.Horizontal) ? 6 : 52;
        }

        protected void ResetHeatMap()
        {
            if (Items != null)
            {
                var bins = Items
                    .OfType<object>()
                    .Select(x => ((DateTime)x.GetType().GetProperty(DateTimeProperty).GetValue(x, null), x))
                    .Where(x => x.Item1.Year == Year)
                    .GroupBy(x => x.Item1.DayOfYear)
                    .Select(x => (x.Key, x.Count()));

                HeatMapSeries.Data = (Orientation == Orientation.Horizontal) ?
                    new double[53, 7] : new double[7, 53];

                DateTime begin = new DateTime(Year, 1, 1);

                foreach (var item in bins)
                {
                    int x = ((int)begin.DayOfWeek + item.Item1 - 1) / 7;
                    int y = ((int)begin.DayOfWeek + item.Item1 - 1) % 7;

                    if (Orientation == Orientation.Horizontal)
                        HeatMapSeries.Data[x, 6 - y] = item.Item2;
                    else
                        HeatMapSeries.Data[y, 52 - x] = item.Item2;
                }
            }
        }

        public static readonly DependencyProperty ItemsSourceProp =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    null,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            if (c.Items != null)
                                c.Items.CollectionChanged -= c.OnItemsChange;

                            c.Items = CollectionViewSource.GetDefaultView(v.NewValue);

                            if (c.Items != null)
                                c.Items.CollectionChanged += c.OnItemsChange;

                            c.UpdateAxes();
                            c.UpdateGrid();
                            c.ResetHeatMap();
                            c.HeatMapPlot.InvalidatePlot();
                        }
                    }
                )
            );

        public static readonly DependencyProperty YearProp =
            DependencyProperty.Register(
                nameof(Year),
                typeof(int),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    DateTime.Now.Year,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            if (c.Items != null)
                                c.Items.Refresh();

                            c.ClearSelected();
                            c.UpdateAxes();
                            c.UpdateGrid();
                            c.ResetHeatMap();
                            c.HeatMapPlot.InvalidatePlot();
                        }
                    }
                )
            );

        public static readonly DependencyProperty SelectionBeginProp =
            DependencyProperty.Register(
                nameof(SelectionBegin),
                typeof(DateTime?),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateGrid();
                            c.HeatMapPlot.InvalidatePlot();
                        }
                    }
                )
            );

        public static readonly DependencyProperty SelectionEndProp =
            DependencyProperty.Register(
                nameof(SelectionEnd),
                typeof(DateTime?),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateGrid();
                            c.HeatMapPlot.InvalidatePlot();
                        }
                    }
                )
            );

        public static readonly DependencyProperty OrientationProp =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateAxes();
                            c.UpdateGrid();
                            c.ResetHeatMap();
                            c.HeatMapPlot.InvalidatePlot();
                        }
                    }
                )
            );
    }
}
