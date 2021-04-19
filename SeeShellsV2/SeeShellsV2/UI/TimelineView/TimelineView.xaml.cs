using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using OxyPlot;
using OxyPlot.Series;
using Unity;

using SeeShellsV2.Data;
using System.Collections.Specialized;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{

    public interface ITimelineViewVM : IViewModel
    {
        ICollectionView ShellEvents { get; }
        ISelected Selected { get; }
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

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ViewModel.Selected.Current = e.AddedCells.Count > 0 ? e.AddedCells[0].Item : null;
        }
    }
}
