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

		public static readonly DependencyProperty ColWidthProp =
			DependencyProperty.Register(
				nameof(ColumnWidth),
				typeof(double),
				typeof(TimelinePanel),
				new PropertyMetadata(170.00, (o, args) => (o as TimelinePanel).InvalidateMeasure()));

		public double ColumnWidth
		{
			get { return (double)GetValue(ColWidthProp); }
			set { SetValue(ColWidthProp,value ); }
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Size panelDesiredSize = new Size();
			foreach (UIElement child in InternalChildren)
			{
				child.Measure(new Size(ColumnWidth, availableSize.Height));
				panelDesiredSize.Width += child.DesiredSize.Width;
				panelDesiredSize.Height = (panelDesiredSize.Height > child.DesiredSize.Height ? 
											panelDesiredSize.Height : child.DesiredSize.Height + 10);
			}

			return panelDesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int i = 0;
			var columnWidth = finalSize.Width / InternalChildren.Count;

			foreach (UIElement child in InternalChildren)
			{
				double x =  (child.DesiredSize.Width * i);
				double y = finalSize.Height - Math.Ceiling(child.DesiredSize.Height);

				var bounds = new Rect(columnWidth * i,  0, columnWidth, finalSize.Height);

				i++;

				child.Arrange(bounds);

			}
			return finalSize; 
		}
	}
}
