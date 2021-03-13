using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SeeShellsV2.UI
{
	public class TimelinePanel : DockPanel
	{ 
		public TimelinePanel() : base()
		{

		}

		//public static readonly DependencyProperty ColWidthProp =
		//	DependencyProperty.Register(
		//		nameof(ColumnWidth),
		//		typeof(double),
		//		typeof(TimelinePanel),
		//		new PropertyMetadata(250.00, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		//public double ColumnWidth
		//{
		//	get { return (double)GetValue(ColWidthProp); }
		//	set { SetValue(ColWidthProp, value); }
		//}

		//public static readonly DependencyProperty ItemsSourceProp =
		//	DependencyProperty.Register(
		//	nameof(ItemsSource),
		//	typeof(IEnumerable),
		//	typeof(TimelinePanel),
		//	new PropertyMetadata(null));

		//public IEnumerable ItemsSource
		//{
		//	get { return (IEnumerable)GetValue(ItemsSourceProp); }
		//	set { SetValue(ItemsSourceProp, value); }
		//}

		protected override Size MeasureOverride(Size availableSize)
		{
			Size panelDesiredSize = new Size();
			foreach (UIElement child in InternalChildren)
			{
				child.Measure(availableSize);
				panelDesiredSize.Width += child.DesiredSize.Width + 18;
				panelDesiredSize.Height = (panelDesiredSize.Height > child.DesiredSize.Height ? 
											panelDesiredSize.Height : child.DesiredSize.Height + 10);
			}

			//Debug.WriteLine(panelDesiredSize.Height);
			//Debug.WriteLine(panelDesiredSize.Width);

			return panelDesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int i = 0;
			double width = 180;
			//foreach (UIElement child in InternalChildren)
			//{
			//	avgWidth += child.DesiredSize.Width;
			//}

			//Debug.WriteLine("AVG" +avgWidth);
			//avgWidth /= InternalChildren.Count;
			//avgWidth = Math.Ceiling(avgWidth);
			//Debug.WriteLine("AVGc " + avgWidth);

			foreach (UIElement child in InternalChildren)
			{
				double x =  (width * i) + 9;
				double y = finalSize.Height - Math.Ceiling(child.DesiredSize.Height);

				i++;

				child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
			}
			return finalSize; // Returns the final Arranged size
		}
	}
}
