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

        private void GenerateTimeline(object sender, RoutedEventArgs e)
        {
            ViewModel.GenerateRandomShellEvents();
        }
    }
}
