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
            container.RegisterSingleton<IShellCollection, ShellCollection>();

            // Register Service Types
            container.RegisterType<ICsvImporter, CsvImporter>();

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
            Window window = container.Resolve<IWindow>("main") as Window;

            // Hack to resolve UI children of main window
            foreach (var child in (window.Content as Visual).GetChildren())
            {
                if (child is Xceed.Wpf.AvalonDock.DockingManager)
                {
                    var dockableEnumerator = (child as Xceed.Wpf.AvalonDock.DockingManager).LogicalChildrenPublic;
                    while (dockableEnumerator.MoveNext())
                    {
                        container.BuildUp(dockableEnumerator.Current.GetType(), dockableEnumerator.Current);

                        foreach (var child2 in (dockableEnumerator.Current as Visual).GetChildren())
                        {
                            container.BuildUp(child2.GetType(), child2);
                        }
                    }
                }
                else
                {
                    container.BuildUp(child.GetType(), child);
                }
            }

            App app = container.Resolve<App>();
            app.Run(window);
        }
    }
}
