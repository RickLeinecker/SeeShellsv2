using System;
using System.Windows;
using System.Windows.Media;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Factories;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;
using SeeShellsV2.UI;

namespace SeeShellsV2
{
    /// <summary>
    /// SeeShellsV2 main entry point
    /// </summary>
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            IUnityContainer container = new UnityContainer();

            // Register Factory Types
            container.RegisterType<IWindowFactory, WindowFactory>();
            container.RegisterType<IShellItemFactory, ShellItemFactory>();

            // Register Repository Types
            container.RegisterSingleton<IShellCollection, ShellCollection>();

            // Register Service Types
            container.RegisterType<IConfigParser, ConfigParser>();
            container.RegisterType<ICsvImporter, CsvImporter>();
            container.RegisterType<IRegistryImporter, RegistryImporter>();

            // Register Window Types
            container.RegisterType<IWindow, MainWindow>("main");

            // Register ViewModel Types
            container.RegisterType<IMainWindowVM, MainWindowVM>();
            container.RegisterType<ITableViewVM, TableViewVM>();

            // Create and run app with main window
            Window window = container.Resolve<IWindow>("main") as Window;

            foreach (var child in (window.Content as Visual).GetChildren())
                container.BuildUp(child.GetType(), child);

            App app = container.Resolve<App>();
            app.Run(window);
        }
    }
}
