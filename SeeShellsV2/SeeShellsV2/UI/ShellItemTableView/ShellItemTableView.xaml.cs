using System;
using System.ComponentModel;
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
    public interface IShellItemTableViewVM : IViewModel
    {
        ISelected Selected { get; }
        IShellItemCollection ShellItems { get; }
    }

    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class ShellItemTableView : UserControl
    {
        [Dependency]
        public IShellItemTableViewVM ViewModel
        {
            get => DataContext as IShellItemTableViewVM;
            set
            {
                PropertyChangedEventHandler deselect = (o, a) => { if (o is ISelected selected && selected.CurrentInspector != ShellItemTable.SelectedItem) ShellItemTable.SelectedItem = null; };

                if (DataContext is IShellItemTableViewVM vm1)
                    vm1.Selected.PropertyChanged -= deselect;

                DataContext = value;

                if (DataContext is IShellItemTableViewVM vm2)
                    vm2.Selected.PropertyChanged += deselect;
            }
        }

        public ShellItemTableView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
                return;

            ViewModel.Selected.CurrentInspector = e.AddedCells[0].Item;
        }
    }
}
