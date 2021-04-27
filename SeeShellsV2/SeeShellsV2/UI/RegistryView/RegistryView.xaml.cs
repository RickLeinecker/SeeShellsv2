using System;
using System.Collections;
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
using System.Collections.Specialized;
using System.Globalization;
using System.ComponentModel;

namespace SeeShellsV2.UI
{
    public interface IRegistryViewVM : IViewModel
    {
        ISelected Selected { get; }
        IShellItemCollection ShellItems { get; }
        IUserCollection Users { get; }
    }

    /// <summary>
    /// Interaction logic for InspectorView.xaml
    /// </summary>
    public partial class RegistryView : UserControl
    {
        [Dependency]
        public IRegistryViewVM ViewModel
        {
            get => DataContext as IRegistryViewVM;
            set
            {
                PropertyChangedEventHandler deselect = (o, a) =>
                {
                    if (o is ISelected selected &&
                        selected.CurrentInspector != RegTreeView.SelectedItem &&
                        RegTreeView.SelectedItem != null)
                    {
                        var container = FindTreeViewSelectedItemContainer(RegTreeView, RegTreeView.SelectedItem);
                        if (container != null)
                        {
                            container.IsSelected = false;
                        }
                    }
                };

                if (DataContext is IRegistryViewVM vm1)
                    vm1.Selected.PropertyChanged -= deselect;

                DataContext = value;

                if (DataContext is IRegistryViewVM vm2)
                    vm2.Selected.PropertyChanged += deselect;
            }
        }

        public RegistryView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
                return;

            ViewModel.Selected.CurrentInspector = e.NewValue;

            if (ViewModel.Selected.CurrentInspector is IShellItem)
                ViewModel.Selected.CurrentData = ViewModel.Selected.CurrentInspector;
            else if (ViewModel.Selected.CurrentInspector is Place p)
                ViewModel.Selected.CurrentData = p.Items.FirstOrDefault();
            else
                ViewModel.Selected.CurrentData = null;
        }

        private static TreeViewItem FindTreeViewSelectedItemContainer(ItemsControl root, object selection)
        {
            if (root == null)
                return null;

            var item = root.ItemContainerGenerator.ContainerFromItem(selection) as TreeViewItem;
            if (item == null)
            {
                foreach (var subItem in root.Items)
                {
                    item = FindTreeViewSelectedItemContainer((TreeViewItem)root.ItemContainerGenerator.ContainerFromItem(subItem), selection);
                    if (item != null)
                    {
                        break;
                    }
                }
            }

            return item;
        }
    }

    internal class RegistryCollection
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public INotifyCollectionChanged Items { get; set; }
    }

    internal class RegistryCollectionsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //get collection name listing...
            string p = parameter as string ?? "";
            var names = p.Split(',').Select(f => f.Trim()).ToList();
            //...and make sure there are no missing entries
            while (2.0 * values.Length > names.Count) names.Add(String.Empty);

            List<RegistryCollection> items = new List<RegistryCollection>();

            int idx = 0;
            foreach (var value in values)
            {
                if (value is INotifyCollectionChanged n)
                    items.Add(new RegistryCollection { Name = names[idx], Icon = names[idx + values.Length], Items = n });

                idx++;
            }

            return items;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
