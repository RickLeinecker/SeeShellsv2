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
    public class TimelinePanel : VirtualizingPanel
	{ 
		public TimelinePanel() : base()
		{

		}

		public TimeSpan Scale
		{
			get { return (TimeSpan)GetValue(ScaleProp); }
			set { SetValue(ScaleProp, value); }
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

		public static readonly DependencyProperty ScaleProp =
			DependencyProperty.Register(
				nameof(ScaleProp),
				typeof(TimeSpan),
				typeof(TimelinePanel),
				new PropertyMetadata(new TimeSpan(1, 0, 0), (o, args) => (o as TimelinePanel).InvalidateMeasure()));


		public static readonly DependencyProperty StartDateProp =
			DependencyProperty.Register(
				nameof(StartDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2020,1,1), (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty EndDateProp =
			DependencyProperty.Register(
				nameof(EndDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2021, 1, 1), (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty MinColumnWidthProp =
			DependencyProperty.Register(
				nameof(MinColumnWidth),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(100.0, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty MaxColumnWidthProp =
			DependencyProperty.Register(
				nameof(MaxColumnWidth),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(250.0, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public static readonly DependencyProperty MaxVisibleElementsProp =
			DependencyProperty.Register(
				nameof(MaxVisibleElements),
				typeof(int),
				typeof(TimelinePanel),
				new PropertyMetadata(25, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

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

		public static DateTime GetDate(IItemContainerGenerator generator, int index)
		{
			DateTime date;
			using (generator.StartAt(generator.GeneratorPositionFromIndex(index), GeneratorDirection.Forward))
			{
				var element = generator.GenerateNext();
				generator.PrepareItemContainer(element);
				date = (DateTime) element.GetValue(DateProperty);
				generator.Remove(generator.GeneratorPositionFromIndex(index), 1);
			}

			return date;
		}

		public static void SetDate(DependencyObject obj, DateTime value)
		{
			obj.SetValue(DateProperty, value);
		}

		private DateTime CalculatedAbsoluteStartDate { get; set; }
		private DateTime CalculatedAbsoluteEndDate { get; set; }
		private TimeSpan CalculatedPixelScale { get; set; }
		private double CalculatedAbsoluteWidth { get; set; }
		private double CalculatedBinWidth { get; set; }
		private int CalculatedBinCount { get; set; }

		protected override Size MeasureOverride(Size availableSize)
		{
			var necessaryChidrenTouchOrGeneratorWillBeNull = Children;
			ItemContainerGenerator generator = ItemContainerGenerator as ItemContainerGenerator;

			if (generator == null || !generator.Items.Any())
				return base.MeasureOverride(availableSize);

			CleanUp();

			// absolute timeline range based on input
			CalculatedAbsoluteStartDate = GetDate(generator, 0);
			CalculatedAbsoluteEndDate = GetDate(generator, generator.Items.Count - 1);

			TimeSpan absoluteScale = CalculatedAbsoluteEndDate.Subtract(CalculatedAbsoluteStartDate);
			CalculatedBinCount = (int)Math.Ceiling(absoluteScale.Ticks / (double)Scale.Ticks);
			CalculatedBinWidth = MaxColumnWidth - (Math.Exp(Scale.Ticks / (double)absoluteScale.Ticks) % (MaxColumnWidth - MinColumnWidth));
			CalculatedAbsoluteWidth = CalculatedBinWidth * CalculatedBinCount;
			CalculatedPixelScale = Scale.Divide(CalculatedBinWidth);

			// compute start and end index for generator (assuming elements are sorted)
			int startIndex = SearchGeneratorItems(StartDate.Subtract(Scale));
			int endIndex = SearchGeneratorItems(EndDate.Add(Scale));
			int itemsCount = endIndex - startIndex;

			GenerateChildren(startIndex, endIndex);

			/*
			if (itemsCount <= MaxVisibleElements)
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
					rect.Width = CalculatedBinWidth;
					rect.Height = 50 * (e - b);
					rect.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x83, 0x0C)); // #0F830C
					SetDate(rect, c);
					AddInternalChild(rect);
				}
			}
			*/

			return new Size { Width = CalculatedAbsoluteWidth, Height = 0.0 };
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (CalculatedBinCount <= 0.0)
				return base.ArrangeOverride(finalSize);

			double[] binsHeights = new double[CalculatedBinCount];

			foreach (UIElement child in InternalChildren)
			{
				DateTime date = GetDate(child);
				int bucket = (int)((date.Subtract(CalculatedAbsoluteStartDate)).Divide(Scale));

				if (bucket >= 0 && bucket < CalculatedBinCount)
				{
					child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
					binsHeights[bucket] += Math.Ceiling(child.DesiredSize.Height);

					double x = CalculatedBinWidth * bucket + ((CalculatedAbsoluteStartDate.Subtract(StartDate)).Divide(CalculatedPixelScale));
					double y = finalSize.Height - binsHeights[bucket];
					double width = CalculatedBinWidth;
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
					r = m-1;
				else
					l = m+1;
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
	}
}
