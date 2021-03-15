using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Globalization;

namespace SeeShellsV2.UI
{
	public class TimelinePanel : Panel
	{ 
		public TimelinePanel() : base()
		{

		}

		public int VisibleElementCount
		{
			get => (int) GetValue(VisibleElementCountProp);
			private set => SetValue(VisibleElementCountProp, value);
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

		public static readonly DependencyProperty VisibleElementCountProp =
			DependencyProperty.Register(
				nameof(VisibleElementCount),
				typeof(int),
				typeof(TimelinePanel),
				new PropertyMetadata(0)
			);

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

		public static readonly DependencyProperty ScaleProp =
			DependencyProperty.Register(
				nameof(ScaleProp),
				typeof(TimeSpan),
				typeof(TimelinePanel),
				new PropertyMetadata(new TimeSpan(1, 0, 0), (o, args) => (o as TimelinePanel).InvalidateMeasure()));

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

		/*
		private void CleanUp()
		{
			IItemContainerGenerator generator = ItemContainerGenerator;

			for (int i = 0; i < InternalChildren.Count; i++)
			{
				UIElement child = InternalChildren[i];
				int index = ((ItemContainerGenerator)generator).IndexFromContainer(child);
				DateTime date = GetDate(child);

				if (date < StartDate || date >= EndDate)
				{
					RemoveInternalChildRange(i, 1);
					generator.Remove(generator.GeneratorPositionFromIndex(index), 1);
					i--;
				}
			}
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			UIElementCollection children = InternalChildren;
			IItemContainerGenerator generator = ItemContainerGenerator;

			CleanUp();

			return base.MeasureOverride(availableSize);
		}
		*/

		protected override Size ArrangeOverride(Size finalSize)
		{
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
		}
	}
}
