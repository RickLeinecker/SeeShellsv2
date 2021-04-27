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
        public ITimelineViewVM ViewModel
        {
            get => DataContext as ITimelineViewVM;
            set
            {
                PropertyChangedEventHandler deselect = (o, a) => { if (o is ISelected selected && selected.CurrentInspector != ShellEventTable.SelectedItem) ShellEventTable.SelectedItem = null; };

                if (DataContext is ITimelineViewVM vm1)
                    vm1.Selected.PropertyChanged -= deselect;

                DataContext = value;

                if (DataContext is ITimelineViewVM vm2)
                    vm2.Selected.PropertyChanged += deselect;
            }
        }

        public TimelineView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
                return;

            ViewModel.Selected.CurrentInspector = e.AddedCells[0].Item;

            if (ViewModel.Selected.CurrentInspector is IShellEvent shellEvent && shellEvent.Evidence.Any())
                ViewModel.Selected.CurrentData = shellEvent.Evidence.First();
        }
    }
}
