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
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using Unity;
using WpfHexaEditor.Core;

namespace SeeShellsV2.UI
{
    public interface IHexViewVM
    {
        public ISelected Selected { get; }
    }

    /// <summary>
    /// Interaction logic for HexView.xaml
    /// </summary>
    public partial class HexView : UserControl
    {
        [Dependency]
        public IHexViewVM ViewModel
        {
            get => DataContext as IHexViewVM;
            set => DataContext = value;
        }

        public HexView()
        {
            InitializeComponent();
        }
    }
}
