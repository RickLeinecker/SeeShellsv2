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
using System.Threading;

namespace SeeShellsV2.UI
{
	public interface ITimelineViewVM : IViewModel
	{
		ISelected Selected { get; }
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

		private Border eventMaybeSelected = null;
		private readonly Stopwatch sw = new Stopwatch();

		private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			sw.Reset();
			sw.Start();
			eventMaybeSelected = sender as Border;

			Task.Run(() =>
			{
				Thread.Sleep(500);
				if (sw.IsRunning)
					sw.Stop();
			});
		}

		private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
			if (sw.IsRunning && sender is Border b && b == eventMaybeSelected)
			{
				sw.Stop();

				if (sw.ElapsedMilliseconds < 300)
					ViewModel.Selected.Current = (sender as FrameworkElement).DataContext;
			}
		}
    }
}
