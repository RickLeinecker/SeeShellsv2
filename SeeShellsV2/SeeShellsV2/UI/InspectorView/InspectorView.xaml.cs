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
    public interface IInspectorViewVM : IViewModel
    {
        ISelected Selected { get; }
    }

    /// <summary>
    /// Interaction logic for InspectorView.xaml
    /// </summary>
    public partial class InspectorView : UserControl
    {
        [Dependency]
        public IInspectorViewVM ViewModel { set => DataContext = value; get => DataContext as IInspectorViewVM; }

        public InspectorView()
        {
            InitializeComponent();
        }

        private void Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
