using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

using Unity;
using Newtonsoft.Json;

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
            // Construct the root container for this application. This container will be used
            // to resolve the dependencies of all object instances throughout the lifetime of the application.
            IUnityContainer container = new UnityContainer();

            // Read Config.JSON to get registry importer settings as an IConfig object.
            Assembly assembly = Assembly.GetExecutingAssembly();
            string internalResourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith("Config.json"));
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(internalResourcePath)))
                container.RegisterInstance<IConfig>(JsonConvert.DeserializeObject<Config>(reader.ReadToEnd()));

            // Register Factory Types
            container.RegisterType<IWindowFactory, WindowFactory>();
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterType<IShellEventFactory, ShellEventFactory>();

            // Register Repository Types
            container.RegisterSingleton<IUserCollection, UserCollection>();
            container.RegisterSingleton<IRegistryHiveCollection, RegistryHiveCollection>();
            container.RegisterSingleton<IShellItemCollection, ShellItemCollection>();
            container.RegisterSingleton<IShellEventCollection, ShellEventCollection>();
            container.RegisterSingleton<ISelected, Selected>();

            // Register Service Types
            container.RegisterType<IPdfExporter, PdfExporter>();
            container.RegisterType<IRegistryImporter, RegistryImporter>();
            container.RegisterType<IShellEventManager, ShellEventManager>();

            // Register Window Types
            container.RegisterType<IWindow, MainWindow>("main");
            container.RegisterType<IWindow, ExportWindow>("export");

            // Register ViewModel Types
            container.RegisterType<IMainWindowVM, MainWindowVM>();
            container.RegisterType<IExportWindowVM, ExportWindowVM>();
            container.RegisterType<IShellItemTableViewVM, ShellItemTableViewVM>();
            container.RegisterType<IInspectorViewVM, InspectorViewVM>();
            container.RegisterType<ITimelineViewVM, TimelineViewVM>();
            container.RegisterType<IRegistryViewVM, RegistryViewVM>();
            container.RegisterType<IFilterControlViewVM, FilterControlViewVM>();
            container.RegisterType<IHexViewVM, HexViewVM>();

            // Create and run app with main window. The main window is contructed in SeeShellsV2.UI.App.OnStartup().
            App app = container.Resolve<App>();
            app.Run();
        }
    }
}
