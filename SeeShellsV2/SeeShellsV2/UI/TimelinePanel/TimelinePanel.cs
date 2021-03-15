using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace SeeShellsV2.UI
{
	public class TimelinePanel : Panel
	{ 
		public TimelinePanel() : base()
		{

		}

		public static readonly DependencyProperty ColWidthProp =
			DependencyProperty.Register(
				nameof(ColumnWidth),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(170.00, (o, args) => (o as TimelinePanel).InvalidateArrange()));

		public double ColumnWidth
		{
			get { return (double)GetValue(ColWidthProp); }
			set { SetValue(ColWidthProp,value ); }
		}

		public static readonly DependencyProperty StartDateProp =
			DependencyProperty.Register(
				nameof(StartDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2020,1,1), (o, args) => (o as TimelinePanel).InvalidateArrange()));

		public DateTime StartDate
		{
			get { return (DateTime)GetValue(StartDateProp); }
			set { SetValue(StartDateProp, value); }
		}

		public static readonly DependencyProperty EndDateProp =
			DependencyProperty.Register(
				nameof(EndDate),
				typeof(DateTime),
				typeof(TimelinePanel),
				new PropertyMetadata(new DateTime(2021, 1, 1), (o, args) => (o as TimelinePanel).InvalidateArrange()));

		public DateTime EndDate
		{
			get { return (DateTime)GetValue(EndDateProp); }
			set { SetValue(EndDateProp, value); }
		}


		public DateTime Begin => new DateTime(2020, 1, 1);
		public DateTime End => new DateTime(2020, 2, 1);

		public static readonly DependencyProperty DateProperty = DependencyProperty.RegisterAttached("Date", typeof(DateTime), typeof(TimelinePanel), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		public static DateTime GetDate(DependencyObject obj)
		{
			return (DateTime)obj.GetValue(DateProperty);
		}

		public static void SetDate(DependencyObject obj, DateTime value)
		{
			obj.SetValue(DateProperty, value);
		}

		//protected override Size MeasureOverride(Size availableSize)
		//{
		//	Size panelDesiredSize = new Size();
		//	foreach (UIElement child in InternalChildren)
		//	{
		//		child.Measure(new Size(ColumnWidth, availableSize.Height));
		//		panelDesiredSize.Width += child.DesiredSize.Width;
		//		panelDesiredSize.Height = (panelDesiredSize.Height > child.DesiredSize.Height ? 
		//									panelDesiredSize.Height : child.DesiredSize.Height + 10);
		//	}

		//	return panelDesiredSize;
		//}

		protected override Size ArrangeOverride(Size finalSize)
		{
			DateTime begin = StartDate;
			DateTime end = EndDate;
			TimeSpan resolution = end - begin;
			int columns = 1;
			var columnWidth = finalSize.Width;


			while (columnWidth / 2 > ColumnWidth)
			{
				columnWidth /= 2;
				columns *= 2;
				resolution = new TimeSpan(resolution.Ticks / 2);
			}

			Debug.WriteLine("CW " + ColumnWidth);
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
					double width = columnWidth;
					double height = Math.Ceiling(child.DesiredSize.Height);
					var bounds = new Rect(x, y, width, height);
					child.Arrange(bounds);
				}

			}
			return finalSize;
		}
	}

	public class DateSliderConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DateTime result = new DateTime();
			if (parameter.ToString().Equals("StartDate"))
			{
				result = new DateTime(2020, 1, 1).AddDays(double.Parse(value.ToString()));

				Debug.WriteLine("Startdateslider "  + result);

			}
			if (parameter.ToString().Equals("EndDate"))
			{
				result = new DateTime(2020, 1, 2).AddDays(double.Parse(value.ToString()));

				Debug.WriteLine("Enddateslider "  + result);

			}			
			
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
