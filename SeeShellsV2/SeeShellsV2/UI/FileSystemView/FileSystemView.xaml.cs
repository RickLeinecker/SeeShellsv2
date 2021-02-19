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

namespace SeeShellsV2.UI
{
    public interface IFileSystemViewVM : IViewModel
    {
        
    }

    /// <summary>
    /// Interaction logic for FileSystemView.xaml
    /// </summary>
    public partial class FileSystemView : UserControl
    {
        [Dependency]
        public IFileSystemViewVM ViewModel { set => DataContext = value; }
        public FileSystemView()
        {
            InitializeComponent();
        }
    }
}
