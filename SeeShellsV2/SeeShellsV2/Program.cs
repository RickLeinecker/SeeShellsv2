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
            container.RegisterType(typeof(IAbstractFactory<IWindow>), typeof(WindowFactory));
            container.RegisterType(typeof(IAbstractFactory<IShellItem>), typeof(ShellItemFactory));

            // Register Repository Types
            container.RegisterSingleton<IShellCollection, ShellCollection>();

            // Register Service Types
            container.RegisterType<ICsvImporter, CsvImporter>();

            // Register Window Types
            container.RegisterType<IWindow, MainWindow>("main");

            // Register ViewModel Types
            container.RegisterType<IMainWindowVM, MainWindowVM>();
            container.RegisterType<ITableViewVM, TableViewVM>();

            // Register Data Types
            container.RegisterType<IShellItem, ShellItem>();
            container.RegisterType<IShellItem, ShellItem0x00>("00");
            container.RegisterType<IShellItem, ShellItem0x1F>("1F");
            container.RegisterType<IShellItem, ShellItem0x20>("20");
            container.RegisterType<IShellItem, ShellItem0x21>("21");
            container.RegisterType<IShellItem, ShellItem0x23>("23");
            container.RegisterType<IShellItem, ShellItem0x25>("25");
            container.RegisterType<IShellItem, ShellItem0x28>("28");
            container.RegisterType<IShellItem, ShellItem0x29>("29");
            container.RegisterType<IShellItem, ShellItem0x2A>("2A");
            container.RegisterType<IShellItem, ShellItem0x2E>("2E");
            container.RegisterType<IShellItem, ShellItem0x2F>("2F");
            container.RegisterType<IShellItem, ShellItem0x30>("30");
            container.RegisterType<IShellItem, ShellItem0x31>("31");
            container.RegisterType<IShellItem, ShellItem0x32>("32");
            container.RegisterType<IShellItem, ShellItem0x34>("34");
            container.RegisterType<IShellItem, ShellItem0x35>("35");
            container.RegisterType<IShellItem, ShellItem0x36>("36");
            container.RegisterType<IShellItem, ShellItem0x40>("40");
            container.RegisterType<IShellItem, ShellItem0x41>("41");
            container.RegisterType<IShellItem, ShellItem0x42>("42");
            container.RegisterType<IShellItem, ShellItem0x43>("43");
            container.RegisterType<IShellItem, ShellItem0x46>("46");
            container.RegisterType<IShellItem, ShellItem0x47>("47");
            container.RegisterType<IShellItem, ShellItem0x4D>("4D");
            container.RegisterType<IShellItem, ShellItem0x4E>("4E");
            container.RegisterType<IShellItem, ShellItem0x61>("61");
            container.RegisterType<IShellItem, ShellItem0x71>("71");
            container.RegisterType<IShellItem, ShellItem0x74>("74");
            container.RegisterType<IShellItem, ShellItem0xB1>("B1");
            container.RegisterType<IShellItem, ShellItem0xC3>("C3");

            // Create and run app with main window
            Window window = container.Resolve<IWindow>("main") as Window;

            foreach (var child in (window.Content as Visual).GetChildren())
                container.BuildUp(child.GetType(), child);

            App app = container.Resolve<App>();
            app.Run(window);
        }
    }
}
