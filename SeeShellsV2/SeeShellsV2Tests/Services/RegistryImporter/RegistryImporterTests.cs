using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShellsV2.Factories;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Unity;

using SeeShellsV2.Data;

namespace SeeShellsV2.Services.Tests
{
    [TestClass()]
    public class RegistryImporterTests
    {
        [TestMethod()]
        public void RegistryImporterTest()
        {
            IUnityContainer container = new UnityContainer();

            IConfig config = new Config
            {
                UsernameLocations = new List<string>() {
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders"
                },
                UserRegistryLocations = new List<string>() {
                    "ntuser.dat",
                    "Local Settings\\Application Data\\Microsoft\\Windows\\UsrClass.dat",
                    "AppData\\Local\\Microsoft\\Windows\\UsrClass.dat"
                },
                ShellbagRootLocations = new List<string> {
                    "Software\\Microsoft\\Windows\\Shell\\BagMRU",
                    "Software\\Microsoft\\Windows\\Shell\\Bags",
                    "Software\\Microsoft\\Windows\\ShellNoRoam\\BagMRU",
                    "Software\\Microsoft\\Windows\\ShellNoRoam\\Bags",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\BagMRU",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\Bags"
                },
                KnownGuids = new Dictionary<string, string>()
            };

            container.RegisterInstance<IConfig>(config);
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterType<IShellItemCollection, ShellItemCollection>();
            container.RegisterInstance<ISelected>(null);

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            Assert.IsTrue(regImporter != null);
        }

        [TestMethod()]
        public void ImportOnlineRegistryTest()
        {
            IUnityContainer container = new UnityContainer();

            IConfig config = new Config
            {
                UsernameLocations = new List<string>() {
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders"
                },
                UserRegistryLocations = new List<string>() {
                    "ntuser.dat",
                    "Local Settings\\Application Data\\Microsoft\\Windows\\UsrClass.dat",
                    "AppData\\Local\\Microsoft\\Windows\\UsrClass.dat"
                },
                ShellbagRootLocations = new List<string> {
                    "Software\\Microsoft\\Windows\\Shell\\BagMRU",
                    "Software\\Microsoft\\Windows\\Shell\\Bags",
                    "Software\\Microsoft\\Windows\\ShellNoRoam\\BagMRU",
                    "Software\\Microsoft\\Windows\\ShellNoRoam\\Bags",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\BagMRU",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\Bags"
                },
                KnownGuids = new Dictionary<string, string>()
            };

            container.RegisterInstance<IConfig>(config);
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterInstance<ISelected>(null);

            ShellItemCollection shellItems = new ShellItemCollection();
            container.RegisterInstance<IShellItemCollection>(shellItems, InstanceLifetime.Singleton);

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            (_, IEnumerable<IShellItem> items) = regImporter.ImportRegistry();

            Assert.IsTrue(shellItems.Count == items.Count());
        }

        [TestMethod()]
        public void ImportOfflineRegistryTest()
        {
            IUnityContainer container = new UnityContainer();

            IConfig config = new Config
            {
                UsernameLocations = new List<string>() {
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders"
                },
                UserRegistryLocations = new List<string>() {
                    "ntuser.dat",
                    "Local Settings\\Application Data\\Microsoft\\Windows\\UsrClass.dat",
                    "AppData\\Local\\Microsoft\\Windows\\UsrClass.dat"
                },
                ShellbagRootLocations = new List<string> {
                    "Software\\Microsoft\\Windows\\Shell\\BagMRU",
                    "Software\\Microsoft\\Windows\\Shell\\Bags",
                    "Software\\Microsoft\\Windows\\ShellNoRoam\\BagMRU",
                    "Software\\Microsoft\\Windows\\ShellNoRoam\\Bags",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\BagMRU",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\Bags"
                },
                KnownGuids = new Dictionary<string, string>()
            };

            container.RegisterInstance<IConfig>(config);
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterInstance<ISelected>(null);

            ShellItemCollection shellItems = new ShellItemCollection();
            container.RegisterInstance<IShellItemCollection>(shellItems, InstanceLifetime.Singleton);

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            (_, IEnumerable<IShellItem> items) = regImporter.ImportRegistry(false, true, "Resources\\UsrClass.dat");

            Assert.IsTrue(shellItems.Count == items.Count());
        }
    }
}