using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShellsV2.Factories;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;

namespace SeeShellsV2.Services.Tests
{
    [TestClass()]
    public class RegistryImporterTests
    {
        [TestMethod()]
        public void RegistryImporterTest()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IConfigParser, ConfigParser>();
            container.RegisterType<IShellItemFactory, ShellItemFactory>();
            container.RegisterType<IShellCollection, ShellCollection>();

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            Assert.IsTrue(regImporter != null);
        }

        [TestMethod()]
        public async Task ImportOnlineRegistryTest()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IConfigParser, ConfigParser>();
            container.RegisterType<IShellItemFactory, ShellItemFactory>();

            ShellCollection shellItems = new ShellCollection();
            container.RegisterInstance<IShellCollection>(shellItems, InstanceLifetime.Singleton);

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            (int parsed, _, _) = await regImporter.ImportOnlineRegistry();

            Assert.IsTrue(shellItems.Count == parsed);
        }

        [TestMethod()]
        public async Task ImportOfflineRegistryTest()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IConfigParser, ConfigParser>();
            container.RegisterType<IShellItemFactory, ShellItemFactory>();

            ShellCollection shellItems = new ShellCollection();
            container.RegisterInstance<IShellCollection>(shellItems, InstanceLifetime.Singleton);

            IRegistryImporter regImporter = container.Resolve<RegistryImporter>();

            (int parsed, _, _) = await regImporter.ImportOfflineRegistry("Resources\\UsrClass.dat");

            Assert.IsTrue(shellItems.Count == parsed);
        }
    }
}