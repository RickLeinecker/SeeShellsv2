using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using System.Globalization;
using System.Diagnostics;

namespace SeeShellsV2.UI
{
	public interface ITimelineViewVM : IViewModel
	{
		IShellEventCollection ShellEvents { get; }

		void GenerateRandomShellEvents();
	}

	/// <summary>
	/// Interaction logic for TimelineView.xaml
	/// </summary>
	public partial class TimelineView : UserControl
	{
		[Dependency]
		public ITimelineViewVM ViewModel { get => DataContext as ITimelineViewVM; set => DataContext = value; }

		public TimelineView()
		{
			InitializeComponent();
		}

		bool initialized = false;
		private void GenerateTimeline(object sender, RoutedEventArgs e)
		{
			if (!initialized)
			{
				initialized = true;
				ViewModel.GenerateRandomShellEvents();
			}
		}

		Point? lastDragPoint;
		private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var mousePos = e.GetPosition(sender as ScrollViewer);
			if (mousePos.X <= (sender as ScrollViewer).ViewportWidth
					&& mousePos.Y < (sender as ScrollViewer).ViewportHeight)
			{
				(sender as ScrollViewer).Cursor = Cursors.SizeAll;
				lastDragPoint = mousePos;
				Mouse.Capture(sender as ScrollViewer);
			}
		}

		private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			(sender as ScrollViewer).Cursor = Cursors.Arrow;
			(sender as ScrollViewer).ReleaseMouseCapture();
			lastDragPoint = null;
		}

		private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
		{
			if (lastDragPoint.HasValue)
			{
				Point posNow = e.GetPosition(sender as ScrollViewer);

				double dX = posNow.X - lastDragPoint.Value.X;
				double dY = posNow.Y - lastDragPoint.Value.Y;

				lastDragPoint = posNow;
				if (Dates.LowerValue != Dates.Minimum && Dates.UpperValue != Dates.Maximum)
				{ 
 					Dates.UpperValue -= dX;
					Dates.LowerValue -= dX;
				}
				else
				{
					if (Dates.UpperValue == Dates.Maximum && Dates.LowerValue != Dates.Minimum  && dX > 0)
					{
						Dates.UpperValue -= dX;
						Dates.LowerValue -= dX;
					}
					else if (Dates.LowerValue == Dates.Minimum && Dates.UpperValue != Dates.Maximum && dX < 0)
					{
						Dates.UpperValue -= dX;
						Dates.LowerValue -= dX;
					}
				}

				//(sender as ScrollViewer).ScrollToHorizontalOffset((sender as ScrollViewer).HorizontalOffset - dX);
				//(sender as ScrollViewer).ScrollToVerticalOffset((sender as ScrollViewer).VerticalOffset - dY);
			}
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
			{
				Dates.LowerValue += 10;
				Dates.UpperValue -= 10;
			}
			if (e.Delta < 0)
			{
				Dates.LowerValue -= 10;
				Dates.UpperValue += 10;
			}
		}
    }
}
