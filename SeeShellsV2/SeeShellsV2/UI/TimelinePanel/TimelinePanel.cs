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

			/*
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
			*/
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
}
