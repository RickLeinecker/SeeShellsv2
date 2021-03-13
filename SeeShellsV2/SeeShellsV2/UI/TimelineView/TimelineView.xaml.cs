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

		private Point scrollMousePoint;
		private double voff = 1;
		private double hoff = 1;

		public TimelineView()
		{
			InitializeComponent();
		}

		private void GenerateTimeline(object sender, RoutedEventArgs e)
		{
			ViewModel.GenerateRandomShellEvents();
		}

		private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			(sender as ScrollViewer).CaptureMouse();
			scrollMousePoint= e.GetPosition(sender as ScrollViewer);
			voff = (sender as ScrollViewer).VerticalOffset;
			hoff = (sender as ScrollViewer).HorizontalOffset;
		}

		private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if ((sender as ScrollViewer).IsMouseCaptured)
			{
				var newHoff = hoff + (scrollMousePoint.X - e.GetPosition(sender as ScrollViewer).X);
				var newVoff = voff + (scrollMousePoint.Y - e.GetPosition(sender as ScrollViewer).Y);
				(sender as ScrollViewer).ScrollToVerticalOffset(newVoff);
				(sender as ScrollViewer).ScrollToHorizontalOffset(newHoff);
			}
		}

		private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			(sender as ScrollViewer).ReleaseMouseCapture();
		}
	}

	public class TimelineSlicer : IMultiValueConverter
	{
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value[0] is IShellEventCollection shellEvents)
				return shellEvents.GroupBy(e => e.TimeStamp.Month);

			return false;
		}

		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
