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
using System.Globalization;

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
			LayoutTransform = new ScaleTransform(0.1, 0.1);
			AbsoluteBeginDate = new DateTime(2020, 1, 1);
			AbsoluteEndDate = new DateTime(2021, 1, 1);
		}

		public double Zoom { get => (double)GetValue(ZoomProp); set => SetValue(ZoomProp, value); }
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

		protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args) { }

		protected override Size MeasureOverride(Size availableSize) 
		{
			var necessaryChidrenTouchOrGeneratorWillBeNull = Children;
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			if (generator == null || !generator.Items.OfType<ITimelineEvent>().Any())
				return new Size { Width = 0, Height = 0 };

			AbsoluteBeginDate = generator.Items.OfType<ITimelineEvent>().First().TimeStamp;
			AbsoluteEndDate = generator.Items.OfType<ITimelineEvent>().Last().TimeStamp;

			CleanUp();

			GenerateChildren();

			if (ScrollOwner != null)
				ScrollOwner.InvalidateScrollInfo();

			return new Size { Width = 0.0, Height = 0.0 };
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (!InternalChildren.OfType<UIElement>().Any())
				return finalSize;

			double minColumnWidth = InternalChildren
				.OfType<UIElement>()
				.Select(c =>
				{
					c.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
					return c.DesiredSize.Width;
				})
				.Max();

			double calculatedBinWidth = Math.Max(finalSize.Width / (VisibleSpan / ColumnSpan), minColumnWidth);
			TimeSpan calculatedPixelSpan = ColumnSpan / calculatedBinWidth;

			int first = (int)((BeginDate - AbsoluteBeginDate) / ColumnSpan);
			int last = (int)((EndDate - AbsoluteBeginDate) / ColumnSpan);

			double[] binsHeights = new double[(int) Resolution];

			foreach (UIElement child in InternalChildren)
			{
				DateTime date = GetDate(child);
				int bucket = (int)((date - AbsoluteBeginDate) / ColumnSpan);

				if (bucket >= first && bucket <= last)
				{
					child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
					binsHeights[bucket] += Math.Ceiling(child.DesiredSize.Height);

					double x = calculatedBinWidth * bucket + ((AbsoluteBeginDate - BeginDate) / calculatedPixelSpan);
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
		}

		public ScrollViewer ScrollOwner { get; set; }
		public bool CanHorizontallyScroll { get; set; }
		public bool CanVerticallyScroll { get; set; }
		public double ExtentWidth => Resolution;
		public double ViewportWidth => VisibleSpan / ColumnSpan;
		public double HorizontalOffset => (BeginDate - AbsoluteBeginDate) / ColumnSpan;
		public double ExtentHeight => 0;
		public double ViewportHeight => 0;
		public double VerticalOffset => 0;

		public void LineDown() { throw new NotImplementedException(); }
		public void LineLeft() { throw new NotImplementedException(); }
		public void LineRight() { throw new NotImplementedException(); }
		public void LineUp() { throw new NotImplementedException(); }
		public Rect MakeVisible(Visual visual, Rect rectangle) { throw new NotImplementedException(); }
		public void MouseWheelDown() { throw new NotImplementedException(); }
		public void MouseWheelLeft() { throw new NotImplementedException(); }
		public void MouseWheelRight() { throw new NotImplementedException(); }
		public void MouseWheelUp() { throw new NotImplementedException(); }
		public void PageDown() { throw new NotImplementedException(); }
		public void PageLeft() { throw new NotImplementedException(); }
		public void PageRight() { throw new NotImplementedException(); }
		public void PageUp() { throw new NotImplementedException(); }
		public void SetHorizontalOffset(double offset) { throw new NotImplementedException(); }
		public void SetVerticalOffset(double offset) { throw new NotImplementedException(); }

		public static readonly DependencyProperty ZoomProp =
			DependencyProperty.Register(
				nameof(Zoom),
				typeof(double),
				typeof(TimelinePanel),
				new FrameworkPropertyMetadata(
					1.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
					FrameworkPropertyMetadataOptions.AffectsMeasure,
					(o, args) => // callback
					{
						TimelinePanel t = o as TimelinePanel;
						t.SetValue(VisibleSpanPropKey, t.AbsoluteSpan / t.Zoom);
						t.SetValue(ScalePropKey, 1.05 - 1 / t.Zoom);
						// t.SetValue(ResolutionPropKey, (double)(1 << (7 + 2 / (int)t.Zoom)));
						t.SetValue(ColumnSpanPropKey, t.AbsoluteSpan / t.Resolution);
					},
					(o, v) => Math.Max((double)v, 1.0) // validation
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
					FrameworkPropertyMetadataOptions.AffectsMeasure,
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

						if (b >= t.AbsoluteEndDate)
							return t.AbsoluteEndDate - new TimeSpan(0, 0, 1);

						if (b < t.AbsoluteBeginDate)
							return t.AbsoluteBeginDate;

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
					FrameworkPropertyMetadataOptions.AffectsMeasure,
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

						if (e > t.AbsoluteEndDate)
							return t.AbsoluteEndDate;

						if (e <= t.AbsoluteBeginDate)
							return t.AbsoluteBeginDate + new TimeSpan(0, 0, 1);

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
							return t.AbsoluteEndDate - new TimeSpan(0, 1, 0);

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
							return t.AbsoluteBeginDate + new TimeSpan(0, 1, 0);

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

						if (value > t.AbsoluteSpan)
							return t.AbsoluteSpan;

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
				new PropertyMetadata(
					0.05,
					(o, args) => // callback
					{
						TimelinePanel t = o as TimelinePanel;
						ScaleTransform s = t.LayoutTransform as ScaleTransform;
						s.ScaleX = s.ScaleY = t.Scale;
					},
					(o, v) => Math.Clamp((double)v, 0.05, 1.0) // validation
				)
			);

		public static readonly DependencyProperty ScaleProp = ScalePropKey.DependencyProperty;

		private static readonly DependencyPropertyKey ResolutionPropKey =
			DependencyProperty.RegisterReadOnly(
				nameof(Resolution),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(512.0)
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
				int index = generator.IndexFromContainer(element);
				DateTime date = GetDate(element);

				if (date < BeginDate || date > EndDate)
				{
					RemoveInternalChildRange(i, 1);

					if (index != -1)
						ItemContainerGenerator.Remove(ItemContainerGenerator.GeneratorPositionFromIndex(index), 1);

					i--;
				}
				else if (element is Rectangle)
				{
					RemoveInternalChildRange(i, 1);
					i--;
				}
			}
		}

		private void GenerateChildren()
		{
			if (ItemContainerGenerator == null)
				return;

			int start = SearchGeneratorItems(BeginDate);
			int end = SearchGeneratorItems(EndDate);

			if (Zoom >= 20.0)
			{
				using (ItemContainerGenerator.StartAt(ItemContainerGenerator.GeneratorPositionFromIndex(start), GeneratorDirection.Forward))
				{
					UIElement child;
					for (int i = start; i < end; i++)
					{
						if (ItemContainerGenerator.GeneratorPositionFromIndex(i).Offset == 0)
							continue;

						child = ItemContainerGenerator.GenerateNext() as UIElement;

						if (child == null)
							continue;

						AddInternalChild(child);
						ItemContainerGenerator.PrepareItemContainer(child);
						SetDate(child, ((ItemContainerGenerator as ItemContainerGenerator).Items[i] as ITimelineEvent).TimeStamp);
					}
				}
			}
			else
			{
				for (var c = BeginDate; c <= EndDate - ColumnSpan; c = c + ColumnSpan)
				{
					int b = SearchGeneratorItems(c);
					int e = SearchGeneratorItems(c + ColumnSpan);

					Rectangle rect = new Rectangle();
					rect.HorizontalAlignment = HorizontalAlignment.Stretch;
					rect.Height = 50 * (e - b);
					rect.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x83, 0x0C)); // #0F830C
					SetDate(rect, c);
					AddInternalChild(rect);
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
