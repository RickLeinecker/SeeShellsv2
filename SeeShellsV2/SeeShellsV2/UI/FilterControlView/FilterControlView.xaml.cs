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
    public interface IFilterControlViewVM : IViewModel
    {
        IShellEventCollection ShellEvents { get; }
        IUserCollection UserCollection { get; }
        IRegistryHiveCollection RegistryHiveCollection { get; }
        User User { get; set; }
        RegistryHive RegistryHive { get; set; }
        DateTime? Begin { get;  set; }
        DateTime? End { get; set; }
        string Path { get; set; }
        Type Type { get; set; }
    }

    /// <summary>
    /// Interaction logic for InspectorView.xaml
    /// </summary>
    public partial class FilterControlView : UserControl
    {
        [Dependency]
        public IFilterControlViewVM ViewModel { set => DataContext = value; }
        public FilterControlView()
        {
            InitializeComponent();
        }

        private void TextBox_KeyEnterUpdate(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tBox = (TextBox)sender;
                DependencyProperty prop = TextBox.TextProperty;

                BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
                if (binding != null) { binding.UpdateSource(); }
                Keyboard.ClearFocus();
            }
        }
    }
}