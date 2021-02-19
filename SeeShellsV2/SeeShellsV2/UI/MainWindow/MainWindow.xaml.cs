using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.Win32;

using SeeShellsV2.Factories;

using Unity;

namespace SeeShellsV2.UI
{
    public interface IMainWindowVM : IViewModel
    {
        public string Title { get; }
        public void ImportFromCSV(string path);
        public void ExportToCSV(string path);
        public Task<(int, int, long)> ImportFromOnlineRegistry();
        public Task<(int, int, long)> ImportFromOfflineRegistry(string hiveLocation);
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindow
    {
        [Dependency]
        public IMainWindowVM ViewModel
        {
            private get { return DataContext as IMainWindowVM; }
            set { DataContext = value; }
        }

        [Dependency]
        public IWindowFactory windowFactory { private get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Import_CSV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                ViewModel.ImportFromCSV(openFileDialog.FileName);
        }

        private void Export_CSV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                ViewModel.ExportToCSV(openFileDialog.FileName);
        }

        private async void Import_Live_Registry_Click(object sender, RoutedEventArgs e)
        {
            (int parsed, int failed, long elapsedMillis) = await ViewModel.ImportFromOnlineRegistry();

            MessageBox.Show(string.Format("{0} shell items parsed, {1} shell items failed, {2} milliseconds.", parsed, failed, elapsedMillis));
        }

        private async void Import_Offline_Registry_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.ReadOnlyChecked = true;
            if (openFileDialog.ShowDialog() == true)
            {
                (int parsed, int failed, long elapsedMillis) = await ViewModel.ImportFromOfflineRegistry(openFileDialog.FileName);
                MessageBox.Show(string.Format("{0} shell items parsed, {1} shell items failed, {2} milliseconds.", parsed, failed, elapsedMillis));
            }
        }
    }
}
