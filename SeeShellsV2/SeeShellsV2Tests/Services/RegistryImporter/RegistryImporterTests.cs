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
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache"
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

            Selected selected = new Selected();
            ShellItemCollection shellItems = new ShellItemCollection();
            UserCollection users = new UserCollection();
            RegistryHiveCollection registries = new RegistryHiveCollection();

            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterInstance<IConfig>(config, InstanceLifetime.Singleton);
            container.RegisterInstance<IShellItemCollection>(shellItems, InstanceLifetime.Singleton);
            container.RegisterInstance<IUserCollection>(users, InstanceLifetime.Singleton);
            container.RegisterInstance<IRegistryHiveCollection>(registries, InstanceLifetime.Singleton);
            container.RegisterInstance<ISelected>(selected, InstanceLifetime.Singleton);

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
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache"
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

            Selected selected = new Selected();
            ShellItemCollection shellItems = new ShellItemCollection();
            UserCollection users = new UserCollection();
            RegistryHiveCollection registries = new RegistryHiveCollection();

            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterInstance<IConfig>(config, InstanceLifetime.Singleton);
            container.RegisterInstance<IShellItemCollection>(shellItems, InstanceLifetime.Singleton);
            container.RegisterInstance<IUserCollection>(users, InstanceLifetime.Singleton);
            container.RegisterInstance<IRegistryHiveCollection>(registries, InstanceLifetime.Singleton);
            container.RegisterInstance<ISelected>(selected, InstanceLifetime.Singleton);

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
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",
                    "Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache"
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

            Selected selected = new Selected();
            ShellItemCollection shellItems = new ShellItemCollection();
            UserCollection users = new UserCollection();
            RegistryHiveCollection registries = new RegistryHiveCollection();
            
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterInstance<IConfig>(config, InstanceLifetime.Singleton);
            container.RegisterInstance<IShellItemCollection>(shellItems, InstanceLifetime.Singleton);
            container.RegisterInstance<IUserCollection>(users, InstanceLifetime.Singleton);
            container.RegisterInstance<IRegistryHiveCollection>(registries, InstanceLifetime.Singleton);
            container.RegisterInstance<ISelected>(selected, InstanceLifetime.Singleton);

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            (_, IEnumerable<IShellItem> items) = regImporter.ImportRegistry(false, true, "Resources\\UsrClass.dat");

            Assert.IsTrue(shellItems.Count == items.Count());
            Assert.IsTrue(registries.Count == 1);
            Assert.IsTrue(registries.First().Places.Distinct().SequenceEqual(registries.First().Places));
        }
    }
}