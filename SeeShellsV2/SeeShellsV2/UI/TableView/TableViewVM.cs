using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using System.Text;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public class TableViewVM : ViewModel, ITableViewVM
    {
        private IShellCollection Collection { get; set; }
        private CollectionViewSource ShellView { get; set; }

        public ICollectionView ShellItems { get => ShellView.View; }

        public TableViewVM([Dependency] IShellCollection collection)
        {
            Collection = collection;
            ShellView = new CollectionViewSource();
            ShellView.Source = Collection;
            ShellView.Filter += (object sender, FilterEventArgs e) => e.Accepted = e.Item as IShellItem != null && (e.Item as IShellItem).Parent == null;
        }
    }
}
