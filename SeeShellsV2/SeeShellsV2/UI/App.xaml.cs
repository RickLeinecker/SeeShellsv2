using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Unity;

using SeeShellsV2.Factories;

namespace SeeShellsV2.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [Dependency]
        public IWindowFactory WindowFactory { get; set; }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            WindowFactory.Create("main").Show();
        }
    }
}
