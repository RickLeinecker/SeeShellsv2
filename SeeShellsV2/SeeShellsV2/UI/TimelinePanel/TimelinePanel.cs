using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;


using System.Windows.Input;
using System.Windows.Media.Animation;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.UI
{
	public interface ITimelineEvent
	{
		public DateTime TimeStamp { get; init; }
	}

	public class TimelinePanel : VirtualizingPanel, IScrollInfo
	{
		public TimelinePanel() : base()
		{
			MaxElementSize = new Size(1.0, 1.0);
			AbsoluteBeginDate = new DateTime(2020, 1, 1);
			AbsoluteEndDate = new DateTime(2021, 1, 1);
			PreviewMouseLeftButtonDown += TimelinePanel_PreviewMouseLeftButtonDown;
			PreviewMouseLeftButtonUp += TimelinePanel_PreviewMouseLeftButtonUp;
			PreviewMouseMove += TimelinePanel_PreviewMouseMove;
			Background = new SolidColorBrush(Colors.Transparent);

			SizeChanged += (_, _) => InvalidateMeasure();
		}

		public double Zoom { get => (double)GetValue(ZoomProp); set => SetValue(ZoomProp, value); }
		public double AnimateBegin { get => (double)GetValue(AnimateBeginProp); set => SetValue(AnimateBeginProp, value); }
		public double AnimateEnd { get => (double)GetValue(AnimateEndProp); set => SetValue(AnimateEndProp, value); }
		public DateTime BeginDate { get => (DateTime)GetValue(BeginDateProp); set => SetValue(BeginDateProp, value); }
		public DateTime EndDate { get => (DateTime)GetValue(EndDateProp); set => SetValue(EndDateProp, value); }

		public DateTime AbsoluteBeginDate { get => (DateTime)GetValue(AbsoluteBeginDateProp); private set => SetValue(AbsoluteBeginDatePropKey, value); }
		public DateTime AbsoluteEndDate { get => (DateTime)GetValue(AbsoluteEndDateProp); private set => SetValue(AbsoluteEndDatePropKey, value); }

		public TimeSpan VisibleSpan { get => (TimeSpan)GetValue(VisibleSpanProp); private set => SetValue(VisibleSpanPropKey, value); }
		public TimeSpan AbsoluteSpan { get => (TimeSpan)GetValue(AbsoluteSpanProp); private set => SetValue(AbsoluteSpanPropKey, value); }

		public TimeSpan ColumnSpan { get => (TimeSpan)GetValue(ColumnSpanProp); }
		public double Scale { get => (double)GetValue(ScaleProp); }
		public double Resolution { get => (double)GetValue(ResolutionProp); }

		protected double ZoomInternal { get => (double)GetValue(ZoomProp); set => SetCurrentValue(ZoomProp, value); }
		protected DateTime BeginDateInternal { get => (DateTime)GetValue(BeginDateProp); set => SetCurrentValue(BeginDateProp, value); }
		protected DateTime EndDateInternal { get => (DateTime)GetValue(EndDateProp); set => SetCurrentValue(EndDateProp, value); }

		private Size MaxElementSize { get; set; }
		private bool ShowCards { get; set; }
		private static bool ContinuousScale => false;

		private void SetResolutionAndScale()
		{
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;
			double newScale = Math.Max(Math.Sqrt(0.1 / Math.Max(1.0, generator.Items.OfType<ITimelineEvent>().Count()) * (Scale * ActualWidth / MaxElementSize.Width) * (AbsoluteSpan / VisibleSpan) * (Scale * ActualHeight / MaxElementSize.Height)), minScale);
			double oldResolution = Resolution;
			double newResolution = (ActualWidth / (newScale / Scale * MaxElementSize.Width)) * (AbsoluteSpan / VisibleSpan);

			if (!ContinuousScale)
				newResolution = 1L << (int)Math.Floor(Math.Log2(newResolution));

			if (newResolution != oldResolution)
			{
				if (!ContinuousScale && newResolution < oldResolution)
					newScale /= 2;

				ShowCards = newScale > 0.8;

				SetValue(ScalePropKey, newScale);
				SetValue(ResolutionPropKey, newResolution);
				SetValue(ColumnSpanPropKey, AbsoluteSpan / Resolution);
			}
		}

		private CancellationTokenSource tokenSource = null;

        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
			ItemContainerGenerator generator = sender as ItemContainerGenerator;

			if (tokenSource != null)
				tokenSource.Cancel();

			tokenSource = new CancellationTokenSource();

			Task.Run(() =>
			{
				Thread.Sleep(777);
				if (!tokenSource.Token.IsCancellationRequested)
					Dispatcher.BeginInvoke((Action)(() =>
					{
						if (generator.Items.OfType<ITimelineEvent>().Any())
						{
							AbsoluteBeginDate = generator.Items.OfType<ITimelineEvent>().First().TimeStamp;
							AbsoluteEndDate = generator.Items.OfType<ITimelineEvent>().Last().TimeStamp;

							BeginDateInternal = AbsoluteBeginDate;
							EndDateInternal = AbsoluteEndDate;
							BeginDateInternal = AbsoluteBeginDate;
						}

						InvalidateMeasure();
					}));
			}, tokenSource.Token);
		}

        protected override bool ShouldItemsChangeAffectLayoutCore(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			return false;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var necessaryChidrenTouchOrGeneratorWillBeNull = Children;
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			if (generator == null || !generator.Items.OfType<ITimelineEvent>().Any())
				return new Size { Width = 0, Height = 0 };

			AbsoluteBeginDate = generator.Items.OfType<ITimelineEvent>().First().TimeStamp;
			AbsoluteEndDate = generator.Items.OfType<ITimelineEvent>().Last().TimeStamp;

			MaxElementSize = new Size(Scale * 168, Scale * 87);

			SetResolutionAndScale();

			CleanUp();

			GenerateChildren();

			MaxElementSize = InternalChildren
				.OfType<UIElement>()
				.Aggregate(new Size(1.0, 1.0), (a, c) =>
				{
					c.Measure(availableSize);
					a.Width = Math.Max(a.Width, c.DesiredSize.Width);
					a.Height = Math.Max(a.Height, c.DesiredSize.Height);
					return a;
				});

			if (!InternalChildren.OfType<UIElement>().Any())
				MaxElementSize = new Size(Scale * 168, Scale * 87);

			if (ScrollOwner != null)
				ScrollOwner.InvalidateScrollInfo();

			return new Size { Width = 0, Height = 0 };
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			columns.Clear();

			if (!InternalChildren.OfType<UIElement>().Any())
				return finalSize;

			foreach (UIElement child in InternalChildren)
			{
				DateTime date = GetDate(child);

				if (date != DateTime.MinValue)
					child.Arrange(GetBounds(date));
				else
					child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
			}

			ExtentHeight = columns.Values.Any() ? columns.Values.Max() : 0;

			return finalSize;
		}

		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);

			DrawTimelineAxis(dc);

			if (!ShowCards)
				DrawRectangles(dc);
		}

		private void DrawRectangles(DrawingContext dc)
		{
			SetResolutionAndScale();

			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			int begin = SearchGeneratorItems(BeginDate - ColumnSpan);
			int end = SearchGeneratorItems(EndDate + ColumnSpan);

			for (int i = begin; i <= end; i++)
				if (i >= 0 && i < generator.Items.Count && generator.Items[i] is ITimelineEvent item)
					dc.DrawRectangle(Brushes.Green, null, GetBounds(item.TimeStamp, Scale * 5.0));

			ExtentHeight = columns.Values.Any() ? columns.Values.Max() : 0;
		}


		private double axisHeight = 32;
		private double axisBorderThickness = 4;
		private double axisLineThickness = 1;
		private double axisFontSize = 12.0;
		private void DrawTimelineAxis(DrawingContext dc)
		{
			FontFamily content = FindResource("MahApps.Fonts.Family.Control") as FontFamily;

			Brush background = FindResource("MahApps.Brushes.ThemeBackground") as Brush;
			Brush foreground = FindResource("MahApps.Brushes.ThemeForeground") as Brush;
			Brush gray = FindResource("MahApps.Brushes.Gray10") as Brush;

			Pen line = new Pen(foreground, axisLineThickness);
			Pen border = new Pen(foreground, axisBorderThickness);

			Typeface font = new Typeface(content, FontStyles.Normal, FontWeights.Normal, FontStretches.Medium);

			dc.DrawRectangle(background, border, new Rect(-axisBorderThickness/2, ActualHeight + axisBorderThickness/2 - axisHeight, ActualWidth + axisBorderThickness, axisHeight));

			FormattedText testText = new FormattedText(
				AbsoluteBeginDate.ToString(),
				CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				font, axisFontSize, foreground, 1.0
			);

			double minlineSpacing = 1.5 * testText.Width;
			double maxVisibleLineCount = ActualWidth / minlineSpacing;

			TimeSpan lineSpan = new TimeSpan(1L << (int)Math.Ceiling(Math.Log2(VisibleSpan.Ticks / maxVisibleLineCount)));
			int begin = (int)((BeginDate - AbsoluteBeginDate).Ticks / lineSpan.Ticks);
			int end = (int)((EndDate - AbsoluteBeginDate).Ticks / lineSpan.Ticks);
			if (begin > end)
				begin = begin.Swap(ref end);
			
			IEnumerable<(int, DateTime)> dates = from x in Enumerable.Range(begin-1, end - begin + 2) select (x, AbsoluteBeginDate + x * lineSpan);

			double lastX = (dates.First().Item2 - BeginDate) / VisibleSpan * ActualWidth;
			foreach ((int idx, DateTime date) in dates)
			{
				if (date < BeginDate - lineSpan || date > EndDate + lineSpan)
					continue;

				FormattedText text = new FormattedText(
					date.ToString(),
					CultureInfo.CurrentCulture,
					FlowDirection.LeftToRight,
					font, axisFontSize, foreground, 1.0
				);

				double x = (date - BeginDate) / VisibleSpan * ActualWidth;

				if (idx % 2 == 0)
					dc.DrawRectangle(gray, null, new Rect(lastX, 0, Math.Abs(x - lastX), ActualHeight - axisHeight - axisBorderThickness));

				dc.DrawLine(line, new Point(x, ActualHeight - axisHeight), new Point(x, ActualHeight - 0.25 * axisHeight));
				dc.DrawText(text, new Point(x + axisBorderThickness, ActualHeight - axisHeight + axisBorderThickness));
				lastX = x;
			}

			if (dates.Last().Item1 % 2 == 1)
				dc.DrawRectangle(gray, null, new Rect(lastX, 0, Math.Abs(ActualWidth - lastX), ActualHeight - axisHeight - axisBorderThickness));
		}

		private Dictionary<long, double> columns = new Dictionary<long, double>();
		private Rect GetBounds(DateTime date, double margin = 0.0)
		{
			double columnWidth = Math.Max(MaxElementSize.Width, (ColumnSpan / VisibleSpan) * ActualWidth);
			TimeSpan calculatedPixelSpan = ColumnSpan / columnWidth;

			long first = (long)Math.Floor((BeginDate - AbsoluteBeginDate) / ColumnSpan);
			long last = (long)Math.Ceiling((EndDate - AbsoluteBeginDate) / ColumnSpan);

			long bucket = (long)((date - AbsoluteBeginDate) / ColumnSpan);

			if (bucket >= first && bucket <= last)
			{
				if (!columns.ContainsKey(bucket))
					columns.Add(bucket, axisHeight + axisBorderThickness);

				columns[bucket] += Math.Ceiling(MaxElementSize.Height);

				double x = margin + columnWidth * bucket - ((BeginDate - AbsoluteBeginDate) / calculatedPixelSpan) + (MaxElementSize.Width == 0.0 ? 0.0 : (columnWidth - MaxElementSize.Width) / 2.0);
				double y = ActualHeight - columns[bucket] + VerticalOffset;
				double width = ((MaxElementSize.Width == 0) ? columnWidth : MaxElementSize.Width) - 2.0 * margin;
				double height = Math.Ceiling(MaxElementSize.Height) - 2.0 * margin;
				return new Rect(x, y, width, height);
			}

			return new Rect(0, 0, 0, 0);
		}

		public ScrollViewer ScrollOwner { get; set; }
		public bool CanHorizontallyScroll { get; set; }
		public bool CanVerticallyScroll { get; set; }
		public double ExtentWidth => AbsoluteSpan / ColumnSpan;
		public double ViewportWidth => VisibleSpan / ColumnSpan;
		public double HorizontalOffset => (BeginDate - AbsoluteBeginDate) / ColumnSpan;
		public double ExtentHeight { get; private set; }
		public double ViewportHeight => ActualHeight;
		public double VerticalOffset {get; private set;}

		private const double LineSize = 10;
		Point? lastDragPoint;
		public void LineUp() { SetVerticalOffset(VerticalOffset - LineSize); }
		public void LineDown() { SetVerticalOffset(VerticalOffset + LineSize); }
		public void LineLeft() { SetHorizontalOffset(HorizontalOffset - LineSize);}
		public void LineRight() { SetHorizontalOffset(HorizontalOffset + LineSize);}
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			return new Rect(HorizontalOffset,
			VerticalOffset, ViewportWidth, ViewportHeight);
		}
		public void MouseWheelDown()
		{
			DoubleAnimation BeginAnimator;
			DoubleAnimation EndAnimator;
			DateTime TempBegin;
			DateTime TempEnd;

			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				SetVerticalOffset(VerticalOffset - LineSize);
				return;
			}
			if (Keyboard.Modifiers == ModifierKeys.Alt)
			{
				SetHorizontalOffset(HorizontalOffset - LineSize);
				return;
			}

			Point p = Mouse.GetPosition(ScrollOwner);
			double NormX = p.X / ActualWidth;

			TempBegin = BeginDateInternal.Subtract((VisibleSpan / 5) * (1 - NormX));
			TempEnd = EndDateInternal.Add((VisibleSpan / 5) * (NormX));

			BeginAnimator = new DoubleAnimation(BeginDateInternal.Ticks, TempBegin.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));
			EndAnimator = new DoubleAnimation(EndDateInternal.Ticks, TempEnd.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));

			BeginAnimation(AnimateBeginProp, BeginAnimator, HandoffBehavior.Compose);
			BeginAnimation(AnimateEndProp, EndAnimator, HandoffBehavior.Compose);

			//if (BeginDateInternal == AbsoluteBeginDate)
			//{
			//	TempEnd = EndDateInternal.Add((VisibleSpan / 10));

			//	EndAnimator = new DoubleAnimation(EndDateInternal.Ticks, TempEnd.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));

			//	BeginAnimation(AnimateEndProp, EndAnimator, HandoffBehavior.Compose);

			//}
			//else if (EndDateInternal == AbsoluteEndDate)
			//{
			//	TempBegin = BeginDateInternal.Subtract((VisibleSpan / 10));

			//	BeginAnimator = new DoubleAnimation(BeginDateInternal.Ticks, TempBegin.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));

			//	BeginAnimation(AnimateBeginProp, BeginAnimator, HandoffBehavior.Compose);
			//}
			//else
			//{
			//	TempBegin = BeginDateInternal.Subtract((VisibleSpan / 5) * (1 - NormX));
			//	TempEnd = EndDateInternal.Add((VisibleSpan / 5) * (NormX));

			//	BeginAnimator = new DoubleAnimation(BeginDateInternal.Ticks, TempBegin.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));
			//	EndAnimator = new DoubleAnimation(EndDateInternal.Ticks, TempEnd.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));

			//	BeginAnimation(AnimateBeginProp, BeginAnimator, HandoffBehavior.Compose);
			//	BeginAnimation(AnimateEndProp, EndAnimator, HandoffBehavior.Compose);
			//}


			//DoubleAnimation ZoomAnimator = new DoubleAnimation(ZoomInternal, ZoomInternal - 5, new Duration(TimeSpan.FromSeconds(0.5/ Math.Sqrt(ZoomInternal))));
			//BeginAnimation(ZoomProp, ZoomAnimator, HandoffBehavior.Compose);
		}
		public void MouseWheelUp()
		{
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				SetVerticalOffset(VerticalOffset + LineSize);
				return;
			}
			if (Keyboard.Modifiers == ModifierKeys.Alt)
			{
				SetHorizontalOffset(HorizontalOffset + LineSize);
				return;
			}

			Point p = Mouse.GetPosition(ScrollOwner);
			double NormX = p.X / ActualWidth;

			DateTime TempBegin = BeginDateInternal.Add((VisibleSpan / 5) * NormX);
			DateTime TempEnd = EndDateInternal.Subtract((VisibleSpan / 5) * (1 - NormX));

			DoubleAnimation BeginAnimator = new DoubleAnimation(BeginDateInternal.Ticks, TempBegin.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));
			DoubleAnimation EndAnimator = new DoubleAnimation(EndDateInternal.Ticks, TempEnd.Ticks, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));

			BeginAnimation(AnimateBeginProp, BeginAnimator, HandoffBehavior.Compose);
			BeginAnimation(AnimateEndProp, EndAnimator, HandoffBehavior.Compose);

			//DoubleAnimation ZoomAnimator = new DoubleAnimation(ZoomInternal, ZoomInternal + 5, new Duration(TimeSpan.FromSeconds(0.5 / Math.Sqrt(ZoomInternal))));
			//BeginAnimation(ZoomProp, ZoomAnimator, HandoffBehavior.Compose);
		}
		public void MouseWheelLeft() { }
		public void MouseWheelRight() { }
		public void PageDown() { SetVerticalOffset(ExtentHeight - ViewportHeight); }
		public void PageLeft() { SetHorizontalOffset(HorizontalOffset - LineSize); }
		public void PageRight() { SetHorizontalOffset(HorizontalOffset + LineSize); }
		public void PageUp() { SetVerticalOffset(0); }

		private void TimelinePanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var mousePos = e.GetPosition(sender as TimelinePanel);
			if (mousePos.X <= (sender as TimelinePanel).ActualWidth
				&& mousePos.Y < (sender as TimelinePanel).ActualHeight)
			{
				(sender as TimelinePanel).Cursor = Cursors.SizeAll;
				lastDragPoint = mousePos;
				Mouse.Capture(sender as TimelinePanel);
			}
		}

		private void TimelinePanel_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (lastDragPoint.HasValue)
			{
				Point posNow = e.GetPosition(sender as TimelinePanel);

				double dX = (posNow.X - lastDragPoint.Value.X);
				double dY = posNow.Y - lastDragPoint.Value.Y;

				lastDragPoint = posNow;


				SetVerticalOffset(VerticalOffset + dY);


				DateTime beginDate = AbsoluteBeginDate + ColumnSpan * (HorizontalOffset - ((dX / ActualWidth) * VisibleSpan) / ColumnSpan);
				DateTime endDate = beginDate + VisibleSpan;

				if (beginDate > BeginDate)
				{
					EndDateInternal = endDate;
					BeginDateInternal = beginDate;
				}
				else
				{
					BeginDateInternal = beginDate;
					EndDateInternal = endDate;
				}

				//if (BeginDateInternal == AbsoluteBeginDate && BeginDate > beginDate)
				//{
				//	EndDateInternal = AbsoluteBeginDate + VisibleSpan;
				//}
				//else if (EndDateInternal == AbsoluteEndDate && EndDate < endDate)
				//{
				//	BeginDateInternal = AbsoluteEndDate - VisibleSpan;
				//}
				//else
				//{
				//	if (beginDate > BeginDate)
				//	{
				//		EndDateInternal = endDate;
				//		BeginDateInternal = beginDate;
				//	}
				//	else
				//	{
				//		BeginDateInternal = beginDate;
				//		EndDateInternal = endDate;
				//	}
				//}
			}
		}

		private void TimelinePanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			(sender as TimelinePanel).Cursor = Cursors.Arrow;
			(sender as TimelinePanel).ReleaseMouseCapture();
			lastDragPoint = null;
		}

		public void SetHorizontalOffset(double offset)
		{
			offset = Math.Clamp(offset, 0, ExtentWidth - ViewportWidth);

			DateTime TempBegin = AbsoluteBeginDate.Add(ColumnSpan * offset);
			DateTime TempEnd = TempBegin.Add(VisibleSpan);

			DoubleAnimation BeginAnimator = new DoubleAnimation(BeginDateInternal.Ticks, TempBegin.Ticks, new Duration(TimeSpan.FromSeconds(0.5)));
			DoubleAnimation EndAnimator = new DoubleAnimation(EndDateInternal.Ticks, TempEnd.Ticks, new Duration(TimeSpan.FromSeconds(0.5)));

			BeginAnimation(AnimateBeginProp, BeginAnimator, HandoffBehavior.Compose);
			BeginAnimation(AnimateEndProp, EndAnimator, HandoffBehavior.Compose);
		}

		public void SetVerticalOffset(double offset)
		{
			offset = Math.Clamp(offset, 0, Math.Max(0, ExtentHeight - ViewportHeight));
			VerticalOffset = offset;
			InvalidateVisual();
		}

		private readonly double minScale = 0.0;
		private readonly double maxScale = 1.0;

		public static readonly DependencyProperty ZoomProp =
			DependencyProperty.Register(
				nameof(Zoom),
				typeof(double),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(
					1.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsRender,
					(o, args) => // callback
					{
						TimelinePanel t = o as TimelinePanel;
						t.SetValue(VisibleSpanPropKey, t.AbsoluteSpan / t.Zoom);
					},
					(o, v) => Math.Max((double)v, 1.0) // validation
				)
			);

		public static readonly DependencyProperty AnimateBeginProp =
		DependencyProperty.Register(
			nameof(AnimateBegin),
			typeof(double),
			typeof(TimelinePanel),
			new FrameworkPropertyMetadata(
				1.0,
				(o, args) => // callback
					{
					TimelinePanel t = o as TimelinePanel;
					t.SetCurrentValue(BeginDateProp, new DateTime((long)t.AnimateBegin));
				}
			)
		);

		public static readonly DependencyProperty AnimateEndProp =
		DependencyProperty.Register(
			nameof(AnimateEnd),
			typeof(double),
			typeof(TimelinePanel),
			new FrameworkPropertyMetadata(
				1.0,
				(o, args) => // callback
				{
					TimelinePanel t = o as TimelinePanel;
					t.SetCurrentValue(EndDateProp, new DateTime((long)t.AnimateEnd));
				}
			)
		);

		public static readonly DependencyProperty BeginDateProp =
			DependencyProperty.Register(
				nameof(BeginDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(
					DateTime.MinValue,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsRender,
					(o, args) => // callback
					{
						TimelinePanel t = o as TimelinePanel;
						t.SetValue(VisibleSpanPropKey, t.EndDate - t.BeginDate);
					},
					(o, v) => // validation
					{
						TimelinePanel t = o as TimelinePanel;
						DateTime b = (DateTime)v;

						if (b >= t.EndDate)
							return t.EndDate - new TimeSpan(0, 0, 1);

						//if (b >= t.AbsoluteEndDate)
						//	return t.AbsoluteEndDate - new TimeSpan(0, 0, 1);

						//if (b < t.AbsoluteBeginDate)
						//	return t.AbsoluteBeginDate;

						return b;
					}
				)
			);

		public static readonly DependencyProperty EndDateProp =
			DependencyProperty.Register(
				nameof(EndDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(
					DateTime.MaxValue,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsRender,
					(o, args) => // callback
					{
						TimelinePanel t = o as TimelinePanel;
						t.SetValue(VisibleSpanPropKey, t.EndDate - t.BeginDate);
					},
					(o, v) => // validation
					{
						TimelinePanel t = o as TimelinePanel;
						DateTime e = (DateTime)v;

						if (e <= t.BeginDate)
							return t.BeginDate + new TimeSpan(0, 0, 1);

						//if (e > t.AbsoluteEndDate)
						//	return t.AbsoluteEndDate;

						//if (e <= t.AbsoluteBeginDate)
						//	return t.AbsoluteBeginDate + new TimeSpan(0, 0, 1);

						return e;
					}
				)
			);

		private static readonly DependencyPropertyKey AbsoluteBeginDatePropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(AbsoluteBeginDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(
					DateTime.MinValue,
					(o, args) => // callback
					{
						TimelinePanel t = (o as TimelinePanel);

						if (t.EndDate < t.AbsoluteBeginDate)
							t.SetCurrentValue(EndDateProp, t.AbsoluteBeginDate);

						if (t.BeginDate < t.AbsoluteBeginDate)
							t.SetCurrentValue(BeginDateProp, t.AbsoluteBeginDate);

						t.SetValue(AbsoluteSpanPropKey, t.AbsoluteEndDate - t.AbsoluteBeginDate);
					},
					(o, v) => // validation
					{
						TimelinePanel t = o as TimelinePanel;
						DateTime b = (DateTime)v;

						if (b >= t.AbsoluteEndDate)
							return t.AbsoluteEndDate - new TimeSpan(0, 0, 1);

						return b;
					}
				)
			);

		public static readonly DependencyProperty AbsoluteBeginDateProp = AbsoluteBeginDatePropKey.DependencyProperty;

		private static readonly DependencyPropertyKey AbsoluteEndDatePropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(AbsoluteEndDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(
					DateTime.MaxValue,
					(o, args) => // callback
					{
						TimelinePanel t = (o as TimelinePanel);

						if (t.BeginDate > t.AbsoluteEndDate)
							t.SetCurrentValue(BeginDateProp, t.AbsoluteEndDate);

						if (t.EndDate > t.AbsoluteEndDate)
							t.SetCurrentValue(EndDateProp, t.AbsoluteEndDate);

						t.SetValue(AbsoluteSpanPropKey, t.AbsoluteEndDate - t.AbsoluteBeginDate);
					},
					(o, v) => // validation
					{
						TimelinePanel t = o as TimelinePanel;
						DateTime e = (DateTime)v;

						if (e <= t.AbsoluteBeginDate)
							return t.AbsoluteBeginDate + new TimeSpan(0, 0, 1);

						return e;
					}
				)
			);

		public static readonly DependencyProperty AbsoluteEndDateProp = AbsoluteEndDatePropKey.DependencyProperty;

		private static readonly DependencyPropertyKey VisibleSpanPropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(VisibleSpan),
				typeof(TimeSpan),
				typeof(TimelinePanel),
				new PropertyMetadata(
					new TimeSpan(0, 0, 1),
					(o, args) => // callback
					{
						TimelinePanel t = (o as TimelinePanel);
						t.SetCurrentValue(ZoomProp, t.AbsoluteSpan / t.VisibleSpan);
						t.SetCurrentValue(EndDateProp, t.BeginDate + t.VisibleSpan);
					},
					(o, v) => // validation
					{
						TimelinePanel t = (o as TimelinePanel);
						TimeSpan value = (TimeSpan)v;
						if (value <= TimeSpan.Zero)
							return new TimeSpan(0, 0, 1);

						//if (value > t.AbsoluteSpan)
						//	return t.AbsoluteSpan;

						return v;
					}
				)
			);

		public static readonly DependencyProperty VisibleSpanProp = VisibleSpanPropKey.DependencyProperty;

		private static readonly DependencyPropertyKey AbsoluteSpanPropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(AbsoluteSpan),
				typeof(TimeSpan),
				typeof(TimelinePanel),
				new PropertyMetadata(
					new TimeSpan(0, 0, 1),
					(o, args) => // callback
					{
						TimelinePanel t = (o as TimelinePanel);
						t.SetCurrentValue(ZoomProp, t.AbsoluteSpan / t.VisibleSpan);
						t.SetValue(AbsoluteEndDatePropKey, t.AbsoluteBeginDate + t.AbsoluteSpan);
					},
					(o, v) => // validation
					{
						TimelinePanel t = (o as TimelinePanel);
						TimeSpan value = (TimeSpan)v;
						if (value <= TimeSpan.Zero)
							return new TimeSpan(0, 0, 1);

						return v;
					}
				)
			);

		public static readonly DependencyProperty AbsoluteSpanProp = AbsoluteSpanPropKey.DependencyProperty;

		private static readonly DependencyPropertyKey ColumnSpanPropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(ColumnSpan),
				typeof(TimeSpan),
				typeof(TimelinePanel),
				new PropertyMetadata(
					new TimeSpan(0, 0, 1),
					(_, _) => { }, // callback
					(o, v) => // validation
					{
						TimelinePanel t = (o as TimelinePanel);
						TimeSpan value = (TimeSpan)v;
						if (value <= TimeSpan.Zero)
							return new TimeSpan(0, 0, 1);

						return v;
					}
				)
			);

		public static readonly DependencyProperty ColumnSpanProp = ColumnSpanPropKey.DependencyProperty;

		private static readonly DependencyPropertyKey ScalePropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(Scale),
				typeof(double),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(
					0.01,
					(o, args) =>
					{
						TimelinePanel t = o as TimelinePanel;
						t.childScale.ScaleX = t.childScale.ScaleY = t.Scale;
					},
					(o, v) => // validation
					{
						TimelinePanel t = o as TimelinePanel;
						return Math.Clamp((double)v, t.minScale, t.maxScale);
					}
				)
			);

		public static readonly DependencyProperty ScaleProp = ScalePropKey.DependencyProperty;

		private static readonly DependencyPropertyKey ResolutionPropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(Resolution),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(
					512.0,
					(_, _) => { },
					(o, v) => Math.Max(1.0, (double)v)
				)
			);

		public static readonly DependencyProperty ResolutionProp = ResolutionPropKey.DependencyProperty;

        private int SearchGeneratorItems(DateTime date)
		{
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			int m = 0;
			int l = 0;
			int r = generator.Items.Count;

			while (r >= l)
			{
				m = l + (r - l) / 2;

				if (m >= generator.Items.Count)
					return generator.Items.Count;

				DateTime test = (generator.Items[m] as ITimelineEvent).TimeStamp;

				if (test == date)
					return m;
				else if (test > date)
					r = m - 1;
				else
					l = m + 1;
			}

			return m;
		}

		private void CleanUp()
		{
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			for (int i = 0; i < InternalChildren.Count; i++)
			{
				UIElement element = InternalChildren[i];
				DateTime date = GetDate(element);

				if (!ShowCards || date < BeginDate || date > EndDate)
				{
					RemoveInternalChildRange(i, 1);
					int index = generator.IndexFromContainer(element);

					if (index != -1)
						ItemContainerGenerator.Remove(ItemContainerGenerator.GeneratorPositionFromIndex(index), 1);

					i--;
				}
			}
		}

		private readonly ScaleTransform childScale = new ScaleTransform();
		private void GenerateChildren()
		{
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			if (generator == null)
				return;

			if (!ShowCards)
				return;

			int begin = SearchGeneratorItems(BeginDate - ColumnSpan);
			int end = SearchGeneratorItems(EndDate + ColumnSpan);

			for (int i = begin; i <= end; i++)
			{
				GeneratorPosition position = ItemContainerGenerator.GeneratorPositionFromIndex(i);

				using (ItemContainerGenerator.StartAt(position, GeneratorDirection.Forward))
				{
					if (ItemContainerGenerator.GenerateNext() is FrameworkElement child)
					{
						if (child == null)
							continue;

						AddInternalChild(child);
						ItemContainerGenerator.PrepareItemContainer(child);
						SetDate(child, (generator.ItemFromContainer(child) as ITimelineEvent).TimeStamp);
						child.SetValue(LayoutTransformProperty, childScale);
					}
				}
			}
		}

		public static readonly DependencyProperty DateProperty =
			DependencyProperty.RegisterAttached(
				"Date",
				typeof(DateTime),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		public static DateTime GetDate(DependencyObject obj)
		{
			return (DateTime)obj.GetValue(DateProperty);
		}

		public static void SetDate(DependencyObject obj, DateTime value)
		{
			obj.SetValue(DateProperty, value);
		}
	}

	/*
	public class TimelinePanel : VirtualizingPanel, IScrollInfo
	{
		public TimelinePanel() : base()
		{
			LayoutTransform = new ScaleTransform(0.1, 0.1);
		}

		public DateTime StartDate
		{
			get { return (DateTime)GetValue(StartDateProp); }
			set { SetValue(StartDateProp, value); }
		}

		public DateTime EndDate
		{
			get { return (DateTime)GetValue(EndDateProp); }
			set { SetValue(EndDateProp, value); }
		}

		public double MinColumnWidth
		{
			get { return (double)GetValue(MinColumnWidthProp); }
			set { SetValue(MinColumnWidthProp, value); }
		}

		public double MaxColumnWidth
		{
			get { return (double)GetValue(MaxColumnWidthProp); }
			set { SetValue(MaxColumnWidthProp, value); }
		}

		public int MaxVisibleElements
		{
			get => (int)GetValue(MaxVisibleElementsProp);
			set => SetValue(MaxVisibleElementsProp, value);
		}

		public TimeSpan Scale
		{
			get => (TimeSpan)GetValue(ScaleProp);
			private set => SetValue(ScaleProp, value);
		}

		public DateTime CalculatedAbsoluteStartDate
		{
			get => (DateTime)GetValue(CalculatedAbsoluteStartDateProp);
			private set => SetValue(CalculatedAbsoluteStartDateProp, value);
		}

		public DateTime CalculatedAbsoluteEndDate
		{
			get => (DateTime)GetValue(CalculatedAbsoluteEndDateProp);
			private set => SetValue(CalculatedAbsoluteEndDateProp, value);
		}

		public static readonly DependencyProperty StartDateProp =
			DependencyProperty.Register(
				nameof(StartDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2010, 1, 1), (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty EndDateProp =
			DependencyProperty.Register(
				nameof(EndDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2022, 1, 1), (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty MinColumnWidthProp =
			DependencyProperty.Register(
				nameof(MinColumnWidth),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(0.0, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty MaxColumnWidthProp =
			DependencyProperty.Register(
				nameof(MaxColumnWidth),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(500.0, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty MaxVisibleElementsProp =
			DependencyProperty.Register(
				nameof(MaxVisibleElements),
				typeof(int),
				typeof(TimelinePanel),
				new PropertyMetadata(50, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty DateProperty =
			DependencyProperty.RegisterAttached(
				"Date",
				typeof(DateTime),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		public static readonly DependencyProperty ScaleProp =
			DependencyProperty.RegisterAttached(
				nameof(Scale),
				typeof(TimeSpan),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(TimeSpan.Zero));

		public static readonly DependencyProperty CalculatedAbsoluteStartDateProp =
			DependencyProperty.Register(
				nameof(CalculatedAbsoluteStartDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2021, 1, 1)));

		public static readonly DependencyProperty CalculatedAbsoluteEndDateProp =
			DependencyProperty.Register(
				nameof(CalculatedAbsoluteEndDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2021, 1, 1)));

		public static DateTime GetDate(DependencyObject obj)
		{
			return (DateTime)obj.GetValue(DateProperty);
		}

		public static DateTime GetDate(IItemContainerGenerator generator, int index)
		{
			index = Math.Clamp(index, 0, (generator as ItemContainerGenerator).Items.Count - 1);

			DateTime date;
			if (generator.GeneratorPositionFromIndex(index).Offset == 0)
				generator.Remove(generator.GeneratorPositionFromIndex(index), 1);

			using (generator.StartAt(generator.GeneratorPositionFromIndex(index), GeneratorDirection.Forward))
			{
				var element = generator.GenerateNext();
				generator.PrepareItemContainer(element);
				date = (DateTime)element.GetValue(DateProperty);
			}

			generator.Remove(generator.GeneratorPositionFromIndex(index), 1);

			return date;
		}

		public static void SetDate(DependencyObject obj, DateTime value)
		{
			obj.SetValue(DateProperty, value);
		}

		// TODO: make scale = 1 / Zoom
		private double Zoom
		{
			get => (LayoutTransform as ScaleTransform).ScaleX;
			set => (LayoutTransform as ScaleTransform).ScaleX = (LayoutTransform as ScaleTransform).ScaleY = value;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var necessaryChidrenTouchOrGeneratorWillBeNull = Children;
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			if (generator == null || !generator.Items.Any())
				return new Size { Width = 0, Height = 0 };

			CleanUp();

			// absolute timeline range based on input
			CalculatedAbsoluteStartDate = GetDate(generator, 0);
			CalculatedAbsoluteEndDate = GetDate(generator, generator.Items.Count - 1);

			Scale = (EndDate.Subtract(StartDate)).Divide(6.0 / Zoom);

			if (Scale.Ticks <= 0)
				return new Size { Width = 0, Height = 0 };

			int startIndex = SearchGeneratorItems(StartDate.Subtract(Scale.Multiply(2.0)));
			int endIndex = SearchGeneratorItems(EndDate.Add(Scale.Multiply(2.0)));
			int itemsCount = endIndex - startIndex;


			if (Zoom >= 0.8)
			{
				GenerateChildren(startIndex, endIndex);
			}
			else
			{
				for (var c = StartDate.Subtract(Scale); c < EndDate.Add(Scale); c = c.Add(Scale))
				{
					int b = SearchGeneratorItems(c);
					int e = SearchGeneratorItems(c.Add(Scale));

					Rectangle rect = new Rectangle();
					rect.HorizontalAlignment = HorizontalAlignment.Stretch;
					rect.Height = 50 * (e - b);
					rect.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x83, 0x0C)); // #0F830C
					SetDate(rect, c);
					AddInternalChild(rect);
				}
			}

			if (ScrollOwner != null)
				ScrollOwner.InvalidateScrollInfo();

			return new Size { Width = 0.0, Height = 0.0 };
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (!InternalChildren.OfType<UIElement>().Any())
				return finalSize;

			if (MinColumnWidth == 0.0)
				SetValue(MinColumnWidthProp, InternalChildren
					.OfType<UIElement>()
					.Select(c =>
					{
						c.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
						return c.DesiredSize.Width;
					})
					.Max());

			TimeSpan absoluteSpan = CalculatedAbsoluteEndDate.Subtract(CalculatedAbsoluteStartDate);

			if (absoluteSpan.Ticks <= 0)
				return finalSize;

			int calculatedBinCount = (int)Math.Ceiling(absoluteSpan.Divide(Scale));
			double calculatedBinWidth = Math.Clamp(finalSize.Width / EndDate.Subtract(StartDate).Divide(Scale), MinColumnWidth, MaxColumnWidth);
			TimeSpan calculatedPixelScale = Scale.Divide(calculatedBinWidth);

			if (calculatedBinCount <= 0.0)
				return finalSize;

			double[] binsHeights = new double[calculatedBinCount];

			foreach (UIElement child in InternalChildren)
			{
				DateTime date = GetDate(child);
				int bucket = (int)((date.Subtract(CalculatedAbsoluteStartDate)).Divide(Scale));

				if (bucket >= 0 && bucket < calculatedBinCount)
				{
					child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
					binsHeights[bucket] += Math.Ceiling(child.DesiredSize.Height);

					double x = calculatedBinWidth * bucket + ((CalculatedAbsoluteStartDate.Subtract(StartDate)).Divide(calculatedPixelScale));
					double y = finalSize.Height - binsHeights[bucket];
					double width = calculatedBinWidth;
					double height = Math.Ceiling(child.DesiredSize.Height);
					var bounds = new Rect(x, y, width, height);
					child.Arrange(bounds);
				}
				else
				{
					child.Arrange(new Rect(0, 0, 0, 0));
				}

			}

			return finalSize;

			/#*
			if (!InternalChildren.OfType<UIElement>().Any())
				return finalSize;

			DateTime begin = StartDate;
			DateTime end = EndDate;
			TimeSpan resolution = end - begin;
			int columns = 1;
			double columnWidth = finalSize.Width;
			double minColWidth = InternalChildren
				.OfType<UIElement>()
				.Select(c =>
				{
					c.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
					return c.DesiredSize.Width;
				})
				.Max();

			while (columnWidth / 2 > minColWidth)
			{
				columnWidth /= 2;
				columns *= 2;
				resolution = new TimeSpan(resolution.Ticks / 2);
			}

			Debug.WriteLine("CW " + minColWidth);
			Debug.WriteLine("cW " + columnWidth);
			Debug.WriteLine("col "+ columns);

			double[] bucketHeights = new double[columns];

			foreach (UIElement child in InternalChildren)
			{
				DateTime date = GetDate(child);
				int bucket = (int)((date.Ticks - begin.Ticks) / resolution.Ticks);

				if (bucket >= 0 && bucket < columns)
				{
					child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
					bucketHeights[bucket] += Math.Ceiling(child.DesiredSize.Height);

					double x = columnWidth * bucket;
					double y = finalSize.Height - bucketHeights[bucket];
					double width = minColWidth;
					double height = Math.Ceiling(child.DesiredSize.Height);
					var bounds = new Rect(x, y, width, height);
					child.Arrange(bounds);
				}
				else
				{
					child.Arrange(new Rect(0, 0, 0, 0));
				}

			}

			return finalSize;
			*#/
		}

		private void CleanUp()
		{
			ItemContainerGenerator.RemoveAll();
			RemoveInternalChildRange(0, InternalChildren.Count);
		}

		private int SearchGeneratorItems(DateTime date)
		{
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			int m = 0;
			int l = 0;
			int r = generator.Items.Count;

			while (r >= l)
			{
				m = l + (r - l) / 2;
				DateTime test = GetDate(generator, m);

				if (test == date)
					return m;
				else if (test > date)
					r = m - 1;
				else
					l = m + 1;
			}

			return m;
		}

		private void GenerateChildren(int start, int end)
		{
			if (ItemContainerGenerator != null)
			{
				using (ItemContainerGenerator.StartAt(ItemContainerGenerator.GeneratorPositionFromIndex(start), GeneratorDirection.Forward))
				{
					UIElement child;
					for (int i = start; i < end; i++)
					{
						child = ItemContainerGenerator.GenerateNext() as UIElement;

						if (child == null)
							break;

						AddInternalChild(child);
						ItemContainerGenerator.PrepareItemContainer(child);
					}
				}
			}
		}

		public ScrollViewer ScrollOwner { get; set; }

		public bool CanHorizontallyScroll
		{
			get => (bool)GetValue(CanHorizontallyScrollProp);
			set => SetValue(CanHorizontallyScrollProp, value);
		}

		public bool CanVerticallyScroll
		{
			get => (bool)GetValue(CanVerticallyScrollProp);
			set => SetValue(CanVerticallyScrollProp, value);
		}

		public double ExtentHeight => 0;

		public double ExtentWidth => (CalculatedAbsoluteEndDate - CalculatedAbsoluteStartDate) / (Scale);

		public double HorizontalOffset => (StartDate - CalculatedAbsoluteStartDate) / (Scale);

		public double VerticalOffset => 0;

		public double ViewportHeight => 0;

		public double ViewportWidth => 6.0 / Zoom;

		public static readonly DependencyProperty CanHorizontallyScrollProp =
			DependencyProperty.Register(
				nameof(CanHorizontallyScroll),
				typeof(bool),
				typeof(TimelinePanel),
				new PropertyMetadata(false, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty CanVerticallyScrollProp =
			DependencyProperty.Register(
				nameof(CanVerticallyScroll),
				typeof(bool),
				typeof(TimelinePanel),
				new PropertyMetadata(false, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public void LineDown()
		{
			
		}

		public void LineLeft()
		{

		}

		public void LineRight()
		{

		}

		public void LineUp()
		{

		}

		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			return rectangle;
		}

		public void MouseWheelDown()
		{
			Zoom = Math.Max(0.1, Zoom - 0.1);
		}

		public void MouseWheelUp()
		{
			Zoom = Math.Min(1.0, Zoom + 0.1);
		}

		public void MouseWheelLeft()
		{

		}

		public void MouseWheelRight()
		{

		}

		public void PageDown()
		{

		}

		public void PageLeft()
		{

		}

		public void PageRight()
		{

		}

		public void PageUp()
		{

		}

		public void SetHorizontalOffset(double offset)
		{
			TimeSpan range = EndDate - StartDate;
			TimeSpan dT = offset / ExtentWidth * (CalculatedAbsoluteEndDate - CalculatedAbsoluteStartDate);
			StartDate = CalculatedAbsoluteStartDate + dT;
			EndDate = StartDate + range;
		}

		public void SetVerticalOffset(double offset)
		{

		}
	}
	*/
}
