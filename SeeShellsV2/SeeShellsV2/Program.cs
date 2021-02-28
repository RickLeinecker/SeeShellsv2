using System;
using System.Collections.Generic;
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
            container.RegisterSingleton<IShellItemCollection, ShellItemCollection>();
            container.RegisterSingleton<IShellEventCollection, ShellEventCollection>();
            container.RegisterSingleton<ISelected, Selected>();

            // Register Service Types
            container.RegisterType<IConfigParser, ConfigParser>();
            container.RegisterType<ICsvImporter, CsvImporter>();
            container.RegisterType<IRegistryImporter, RegistryImporter>();

            // Register Window Types
            container.RegisterType<IWindow, MainWindow>("main");
            container.RegisterType<IWindow, ExportWindow>("export");

            // Register ViewModel Types
            container.RegisterType<IMainWindowVM, MainWindowVM>();
            container.RegisterType<IExportWindowVM, ExportWindowVM>();
            container.RegisterType<ITableViewVM, TableViewVM>();
            container.RegisterType<IInspectorViewVM, InspectorViewVM>();
            container.RegisterType<ITimelineViewVM, TimelineViewVM>();
            container.RegisterType<IRegistryViewVM, RegistryViewVM>();
            container.RegisterType<IFileSystemViewVM, FileSystemViewVM>();
            container.RegisterType<IFilterControlViewVM, FilterControlViewVM>();

            // Create and run app with main window
            App app = container.Resolve<App>();

            IWindowFactory windowFactory = container.Resolve<IWindowFactory>();
            Window mainWindow = windowFactory.Create("main") as Window;

            app.Run(mainWindow);
        }
    }
}
