using System;
using System.Collections.Generic;
using System.Text;

using Unity;

using SeeShellsV2.Repositories;
using SeeShellsV2.Services;

namespace SeeShellsV2.UI
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        [Dependency] public ICsvImporter importer { get; set; }

        public string Title { get { return "SeeShells"; } }

        private IInspectorViewVM inspector = new InspectorViewVM();
        [Dependency]
        public IInspectorViewVM InspectorView
        {
            get => inspector;
            set
            {
                inspector = value;
                NotifyPropertyChanged();
            }
        }


        private ITimelineViewVM timeline = new TimelineViewVM();
        [Dependency]
        public ITimelineViewVM TimelineView
        {
            get => timeline;
            set
            {
                timeline = value;
                NotifyPropertyChanged();
            }
        }


        private ITableViewVM table = new TableViewVM();
        [Dependency]
        public ITableViewVM TableView
        {
            get => table;
            set
            {
                table = value;
                NotifyPropertyChanged();
            }
        }

        private IFileSystemViewVM file = new FileSystemViewVM();
        [Dependency]
        public IFileSystemViewVM FileView
        {
            get => file;
            set
            {
                file = value;
                NotifyPropertyChanged();
            }
        }

        private IRegistryViewVM registry = new RegistryViewVM();
        [Dependency]
        public IRegistryViewVM RegistryView
        {
            get => registry;
            set
            {
                registry = value;
                NotifyPropertyChanged();
            }
        }

        private IFilterControlViewVM filtercontrol = new FilterControlViewVM();
        [Dependency]
        public IFilterControlViewVM FilterControlView
        {
            get => filtercontrol;
            set
            {
                filtercontrol = value;
                NotifyPropertyChanged();
            }
        }

        public void ImportFromCSV(string path)
        {
            // importer.Import(path); => needs work
        }

        public void ExportWindow()
        {
            ExportWindow win = new ExportWindow();
            win.Show();
        }

        public void ExportToCSV(string path)
        {

        }
    }
}
