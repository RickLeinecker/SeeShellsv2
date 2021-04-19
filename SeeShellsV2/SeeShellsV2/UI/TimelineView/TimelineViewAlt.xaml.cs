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

    public interface ITimelineViewAltVM : IViewModel
    {
        ICollectionView ShellEvents { get; }
        ISelected Selected { get; }
    }

    /// <summary>
    /// Interaction logic for TimelineViewAlt.xaml
    /// </summary>
    public partial class TimelineViewAlt : UserControl
    {
        [Dependency]
        public ITimelineViewAltVM ViewModel { get => DataContext as ITimelineViewAltVM; set => DataContext = value; }

        public TimelineViewAlt()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ViewModel.Selected.Current = e.AddedCells.Count > 0 ? e.AddedCells[0].Item : null;
        }
    }
}
