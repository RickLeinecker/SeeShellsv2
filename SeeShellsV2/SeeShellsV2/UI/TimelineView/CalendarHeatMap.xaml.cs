using System;
using System.Collections;
using System.Collections.Generic;
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
using OxyPlot.Wpf;

namespace SeeShellsV2.UI
{
    public partial class CalendarHeatMap : UserControl
    {
        /// <summary>
        /// The year that is currently being displayed on the heatmap
        /// </summary>
        public int Year { get => (int)GetValue(YearProp); set => SetValue(YearProp, value); }

        /// <summary>
        /// The first date in the currently selected date range
        /// </summary>
        public DateTime? SelectionBegin { get => GetValue(SelectionBeginProp) as DateTime?; set => SetValue(SelectionBeginProp, value); }

        /// <summary>
        /// The last date in the currently selected date range
        /// </summary>
        public DateTime? SelectionEnd { get => GetValue(SelectionEndProp) as DateTime?; set => SetValue(SelectionEndProp, value); }

        /// <summary>
        /// The data to be displayed on the heatmap
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProp) as IEnumerable;
            set => SetValue(ItemsSourceProp, value);
        }

        /// <summary>
        /// View of the data to be displayed on the heatmap
        /// </summary>
        private ICollectionView Items { get; set; }

        /// <summary>
        /// The name of the DateTime property to use when calculating frequency data.
        /// Items bound to ItemsSource should have a DateTime property with this name
        /// </summary>
        public string DateTimeProperty
        {
            get => GetValue(DateTimePropertyProp) as string;
            set => SetValue(DateTimePropertyProp, value);
        }

        /// <summary>
        /// The title displayed with the color axis
        /// </summary>
        public string ColorAxisTitle
        {
            get => GetValue(ColorAxisTitleProp) as string;
            set => SetValue(ColorAxisTitleProp, value);
        }

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProp);
            set => SetValue(TextColorProp, value);
        }

        public Color GridColor
        {
            get => (Color) GetValue(GridColorProp);
            set => SetValue(GridColorProp, value);
        }

        public Color SelectionColor
        {
            get => (Color) GetValue(SelectionColorProp);
            set => SetValue(SelectionColorProp, value);
        }

        /// <summary>
        /// The orientation of the chart. can be displayed horizontally or vertically
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProp);
            set => SetValue(OrientationProp, value);
        }

        public bool AllowInteraction { get => _allowInteraction; set => _allowInteraction = value; }

        private bool _allowInteraction = true;
        private DateTime? _last_selected = null;
        private readonly string[] weekdays = new string[] { "S", "M", "T", "W", "T", "F", "S" };
        private readonly string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        /// <summary>
        /// construct a new heat map
        /// </summary>
        public CalendarHeatMap()
        {
            InitializeComponent();
            (HeatMapSeries.InternalSeries as OxyPlot.Series.HeatMapSeries).Interpolate = false;
        }

        /// <summary>
        /// reset the date range selection
        /// </summary>
        public void ClearSelected()
        {
            _last_selected = null;
            SetCurrentValue(SelectionBeginProp, null);
            SetCurrentValue(SelectionEndProp, null);
        }

        private void OnSizeChanged(object o, SizeChangedEventArgs sizeInfo)
        {
            if (sizeInfo.NewSize.Width >= Math.Max(1, (o as Grid).RowDefinitions[1].ActualHeight))
            {
                if (Orientation == Orientation.Vertical)
                    Orientation = Orientation.Horizontal;

                double aspect = 750.04 / 204.619;
                HeatMapPlot.SetCurrentValue(WidthProperty, double.NaN);
                HeatMapPlot.SetCurrentValue(HeightProperty, sizeInfo.NewSize.Width / aspect);
            }
            else
            {
                if (Orientation == Orientation.Horizontal)
                    Orientation = Orientation.Vertical;

                double aspect = 750.04 / 204.619;
                HeatMapPlot.SetCurrentValue(WidthProperty, (o as Grid).RowDefinitions[1].ActualHeight / aspect);
                HeatMapPlot.SetCurrentValue(HeightProperty, double.NaN);
            }
        }

        private void OnItemsChange(object sender, NotifyCollectionChangedEventArgs args) => ResetHeatMap();
        private void Clear_MenuItem_Click(object sender, RoutedEventArgs e) => ClearSelected();
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => e.Handled = !AllowInteraction;
        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e) => ClearSelected();
        private void Left_Button_Click(object sender, RoutedEventArgs e) => SetCurrentValue(YearProp, Year - 1);
        private void Right_Button_Click(object sender, RoutedEventArgs e) => SetCurrentValue(YearProp, Year + 1);
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            DateTime date;
            double i, j;

            HeatMapPlot.HideTracker();

            if (!AllowInteraction)
            {
                e.Handled = true;
                return;
            }

            if (Mouse.RightButton == MouseButtonState.Released)
            {
                _last_selected = null;
                return;
            }

            OxyPlot.OxyMouseEventArgs args = ConverterExtensions.ToMouseEventArgs(e, sender as IInputElement);

            if (!HeatMapPlot.ActualModel.PlotArea.Contains(args.Position))
                return;

            HeatMapPlot.ShowTracker(HeatMapSeries.InternalSeries.GetNearestPoint(args.Position, true));

            if (Orientation == Orientation.Vertical)
            {
                i = Math.Ceiling(DayAxis.InternalAxis.InverseTransform(args.Position.X) - 0.5);
                j = Math.Ceiling(WeekAxis.InternalAxis.InverseTransform(args.Position.Y) - 0.5);
                date = new DateTime(Year, 1, 1).AddDays((52 - j) * 7 + i - (int)new DateTime(Year, 1, 1).DayOfWeek);
            }
            else
            {
                i = Math.Ceiling(WeekAxis.InternalAxis.InverseTransform(args.Position.X) - 0.5);
                j = Math.Ceiling(DayAxis.InternalAxis.InverseTransform(args.Position.Y) - 0.5);
                date = new DateTime(Year, 1, 1).AddDays(i * 7 + (6 - j) - (int)new DateTime(Year, 1, 1).DayOfWeek);
            }

            UpdateSelection(date);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            HeatMapPlot.InvalidatePlot();

            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// attempt to add a date to the selected date range
        /// </summary>
        /// <param name="date">the date to add to the selection</param>
        protected void UpdateSelection(DateTime date)
        {
            if (date.Year != Year)
                return;

            if (_last_selected != null && date == _last_selected)
                return;

            _last_selected = date;

            if (SelectionBegin == null || SelectionEnd == null)
            {
                SetCurrentValue(SelectionBeginProp, date);
                SetCurrentValue(SelectionEndProp, date.AddDays(1));
            }
            else if (_last_selected == SelectionEnd || date > SelectionBegin)
            {
                SetCurrentValue(SelectionEndProp, date.AddDays(1));
            }
            else if (_last_selected == SelectionBegin || date < SelectionEnd)
            {
                SetCurrentValue(SelectionBeginProp, date);
            }
        }

        /// <summary>
        /// clear and redraw the heatmap grid, including the selection and month separators
        /// </summary>
        protected void UpdateGrid()
        {
            HeatMapPlot.Annotations.Clear();

            HeatMapPlot.PlotAreaBorderColor = TextColor;
            HeatMapPlot.PlotAreaBorderThickness = new Thickness(1);

            for (int i = 0; i < 6; i++)
                HeatMapPlot.Annotations.Add(new LineAnnotation
                {
                    X = (Orientation == Orientation.Vertical) ? 0.5 + i : 0,
                    Y = (Orientation == Orientation.Vertical) ? 0 : 0.5 + i,
                    Type = (Orientation == Orientation.Vertical) ?
                        OxyPlot.Annotations.LineAnnotationType.Vertical : OxyPlot.Annotations.LineAnnotationType.Horizontal,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    Color = GridColor
                });

            for (int i = 0; i < 53; i++)
                HeatMapPlot.Annotations.Add(new LineAnnotation
                {
                    X = (Orientation == Orientation.Horizontal) ? 0.5 + i : 0,
                    Y = (Orientation == Orientation.Horizontal) ? 0 : 0.5 + i,
                    Type = (Orientation == Orientation.Horizontal) ?
                        OxyPlot.Annotations.LineAnnotationType.Vertical : OxyPlot.Annotations.LineAnnotationType.Horizontal,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    Color = GridColor
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
                        Stroke = TextColor,
                        Fill = Colors.Transparent,
                        StrokeThickness = 1
                    });
                }
                else
                {
                    HeatMapPlot.Annotations.Add(new PolygonAnnotation
                    {
                        Points = new List<OxyPlot.DataPoint>
                        {
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1 + 7) / 7 - 0.5, 6.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) / 7 + 0.5, 6 - (((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) / 7 - 0.5, 6 - (((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) / 7 - 0.5, -0.5),

                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1 - 7) / 7 + ((i == 1) ?  -0.5 : 0.5), -0.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) / 7 - 0.5, 6 - (((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) / 7 + 0.5, 6 - (((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1 + 7) / 7 - 0.5, 6.5),
                        },
                        LineStyle = OxyPlot.LineStyle.Solid,
                        Stroke = TextColor,
                        Fill = Colors.Transparent,
                        StrokeThickness = 1
                    });
                }
            }

            RangeClear.Visibility = (SelectionBegin != null && SelectionEnd != null) ? Visibility.Visible : Visibility.Collapsed;
            RangeDisplay.Text = (SelectionBegin != null && SelectionEnd != null) ? SelectionBegin?.ToShortDateString() + " - " + SelectionEnd?.AddDays(-1).ToShortDateString() : string.Empty;

            if (SelectionBegin is DateTime start && SelectionEnd is DateTime end)
            {
                int p0 = start.DayOfYear;
                int p1 = end.AddDays(-1).DayOfYear;

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
                        },
                        Fill = SelectionColor
                    });
                }
                else
                {
                    HeatMapPlot.Annotations.Add(new PolygonAnnotation
                    {
                        Points = new List<OxyPlot.DataPoint>
                        {
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1 + 7) / 7 - 0.5, 6.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) / 7 + 0.5, 6 - (((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) / 7 - 0.5, 6 - (((int)begin.DayOfWeek + p1 - 1) % 7 + 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p1 - 1) / 7 - 0.5, -0.5),

                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1 - 7) / 7 + (((p0 + (int)begin.DayOfWeek) <= 6) ?  -0.5 : 0.5), -0.5),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) / 7 - 0.5, 6 - (((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1) / 7 + 0.5, 6 - (((int)begin.DayOfWeek + p0 - 1) % 7 - 0.5)),
                            new OxyPlot.DataPoint(((int)begin.DayOfWeek + p0 - 1 + 7) / 7 - 0.5, 6.5),
                        },
                        Fill = SelectionColor
                    });
                }
            }
        }

        /// <summary>
        /// Set axis bounds, labels
        /// </summary>
        protected void UpdateAxes()
        {
            HeatMapTitle.Text = Year.ToString();

            HeatMapPlot.TitleColor = TextColor;

            ColorAxis.TitleColor = TextColor;
            ColorAxis.TextColor = TextColor;
            ColorAxis.TicklineColor = Colors.Transparent;

            ColorAxis.Title = ColorAxisTitle;

            ColorAxis.Position = (Orientation == Orientation.Horizontal) ?
                OxyPlot.Axes.AxisPosition.Top : OxyPlot.Axes.AxisPosition.Right;

            DayAxis.TitleColor = TextColor;
            DayAxis.TextColor = TextColor;
            DayAxis.TicklineColor = Colors.Transparent;

            DayAxis.Position = (Orientation == Orientation.Horizontal) ?
                OxyPlot.Axes.AxisPosition.Left : OxyPlot.Axes.AxisPosition.Bottom;

            DayAxis.ItemsSource = (Orientation == Orientation.Horizontal) ?
                weekdays.Reverse() : weekdays;

            WeekAxis.TitleColor = TextColor;
            WeekAxis.TextColor = TextColor;
            WeekAxis.TicklineColor = Colors.Transparent;

            WeekAxis.Position = (Orientation == Orientation.Horizontal) ?
                OxyPlot.Axes.AxisPosition.Top : OxyPlot.Axes.AxisPosition.Right;

            WeekAxis.Minimum = -0.5;
            WeekAxis.Maximum = 52.5;

            MonthAxis.TitleColor = TextColor;
            MonthAxis.TextColor = TextColor;
            MonthAxis.TicklineColor = Colors.Transparent;

            MonthAxis.Position = (Orientation == Orientation.Horizontal) ?
                OxyPlot.Axes.AxisPosition.Bottom : OxyPlot.Axes.AxisPosition.Left;

            MonthAxis.ItemsSource = (Orientation == Orientation.Horizontal) ?
                months : months.Reverse();

            HeatMapSeries.X0 = 0;
            HeatMapSeries.X1 = (Orientation == Orientation.Horizontal) ? 52 : 6;
            HeatMapSeries.Y0 = 0;
            HeatMapSeries.Y1 = (Orientation == Orientation.Horizontal) ? 6 : 52;
        }


        private CancellationTokenSource _tokenSource = null;
        private readonly object _resetLock = new object();
        /// <summary>
        /// Clear and recalculate frequency data
        /// </summary>
        protected void ResetHeatMap(int delay = 200)
        {
            if (_tokenSource != null)
                _tokenSource.Cancel();

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            var orientation = Orientation;
            var year = Year;
            var dateProp = DateTimeProperty;

            // run the update with a small delay to prevent rapid fire updates.
            Task.Run(() =>
            {
                Thread.Sleep(delay);

                if (token.IsCancellationRequested)
                    return;

                if (Items != null)
                {
                    var bins = Items
                        .OfType<object>()
                        .Select(x => ((DateTime)x.GetType().GetProperty(dateProp).GetValue(x, null), x))
                        .Where(x => x.Item1.Year == year)
                        .GroupBy(x => x.Item1.DayOfYear)
                        .Select(x => (x.Key, x.Count()));

                    double[,] data = (orientation == Orientation.Horizontal) ?
                        new double[53, 7] : new double[7, 53];

                    DateTime begin = new DateTime(year, 1, 1);
                    try
                    {
                        foreach (var item in bins)
                        {
                            int x = ((int)begin.DayOfWeek + item.Key - 1) / 7;
                            int y = ((int)begin.DayOfWeek + item.Key - 1) % 7;

                            if (orientation == Orientation.Horizontal)
                                data[x, 6 - y] = item.Item2;
                            else
                                data[y, 52 - x] = item.Item2;
                        }

                        lock (_resetLock)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                HeatMapSeries.Data = data;
                                InvalidateMeasure();
                            });
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // collection was updated, so abort this update and wait for the next
                    }
                }
            }, token);
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
                        }
                    }
                )
            );

        public static readonly DependencyProperty DateTimePropertyProp =
            DependencyProperty.Register(
                nameof(DateTimeProperty),
                typeof(string),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    null,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateAxes();
                            c.UpdateGrid();
                            c.ResetHeatMap();
                        }
                    }
                )
            );

        public static readonly DependencyProperty ColorAxisTitleProp =
            DependencyProperty.Register(
                nameof(ColorAxisTitle),
                typeof(string),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateAxes();
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
                            c.UpdateGrid();
                            c.UpdateAxes();
                            c.ClearSelected();
                            c.ResetHeatMap(0);
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
                    null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateGrid();
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
                    null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateGrid();
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
                    Orientation.Horizontal,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c && v.NewValue is Orientation orientation)
                        {
                            c.UpdateAxes();
                            c.UpdateGrid();
                            c.ResetHeatMap();
                        }
                    }
                )
            );

        public static readonly DependencyProperty TextColorProp =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    Colors.Black, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateAxes();
                            c.UpdateGrid();
                        }
                    }
                )
            );

        public static readonly DependencyProperty GridColorProp =
            DependencyProperty.Register(
                nameof(GridColor),
                typeof(Color),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    Colors.DarkGray, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateAxes();
                            c.UpdateGrid();
                        }
                    }
                )
            );

        public static readonly DependencyProperty SelectionColorProp =
            DependencyProperty.Register(
                nameof(SelectionColor),
                typeof(Color),
                typeof(CalendarHeatMap),
                new FrameworkPropertyMetadata(
                    Colors.Yellow,  FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, v) =>
                    {
                        if (o is CalendarHeatMap c)
                        {
                            c.UpdateAxes();
                            c.UpdateGrid();
                        }
                    }
                )
            );
    }

    internal class CalendarHeatMapTrackerTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&  values[0] is OxyPlot.TrackerHitResult result && values[1] is CalendarHeatMap c)
            {
                IList<(string, string)> trakerText = result.Text
                    .Split('\n')
                    .Skip(1)
                    .Select(s => s.Split(": "))
                    .Select(s => (s[0], s[1]))
                    .ToList();

                int offset = (int) (c.Orientation == Orientation.Horizontal ?
                    (6 - result.DataPoint.Y) + 7 * result.DataPoint.X : result.DataPoint.X + 7 * (52 - result.DataPoint.Y));

                DateTime date = new DateTime(c.Year, 1, 1);
                date = date.AddDays(offset - (int) date.DayOfWeek);

                return date.ToShortDateString() + '\n' + "Count: " + trakerText[2].Item2;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
