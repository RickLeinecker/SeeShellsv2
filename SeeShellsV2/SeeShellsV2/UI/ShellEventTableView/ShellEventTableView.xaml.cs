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

using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public interface IShellEventTableViewVM : IViewModel
    {
        public IShellEventCollection ShellEvents { get; }

        public ISelected Selected { get; }
    }

    /// <summary>
    /// Interaction logic for ShellEventTableView.xaml
    /// </summary>
    public partial class ShellEventTableView : UserControl
    {
        [Dependency]
        public IShellEventTableViewVM ViewModel
        {
            get => DataContext as IShellEventTableViewVM;
            set => DataContext = value;
        }

        public ShellEventTableView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ViewModel.Selected.Current = e.AddedCells.Count > 0 ? e.AddedCells[0].Item : null;
        }
    }
}
