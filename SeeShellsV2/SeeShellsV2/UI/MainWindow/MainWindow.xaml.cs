using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Win32;

using MahApps.Metro.Controls;
using Unity;

using SeeShellsV2.Factories;

namespace SeeShellsV2.UI
{
    public interface IMainWindowVM : IViewModel
    {
        //public void ImportFromCSV(string path);
        //void ExportToCSV(string path);
        bool ImportFromRegistry(string hiveLocation = null);
        void RestartApplication(bool runAsAdmin = false);
        string WebsiteUrl { get; }
        string GithubUrl { get; }

        Visibility StatusVisibility { get; }
        string Status { get; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IWindow
    {
        [Dependency]
        public IMainWindowVM ViewModel
        {
            private get { return DataContext as IMainWindowVM; }
            set { DataContext = value; }
        }

        [Dependency]
        public IWindowFactory WindowFactory { private get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        //private void Import_CSV_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
        //    if (openFileDialog.ShowDialog() == true)
        //        ViewModel.ImportFromCSV(openFileDialog.FileName);
        //}

        private void Export_Window_Click(object sender, RoutedEventArgs e)
        {
            IWindow win = WindowFactory.Create("export");
            win.Show();
        }

		//private void Export_CSV_Click(object sender, RoutedEventArgs e)
		//{
		//	OpenFileDialog openFileDialog = new OpenFileDialog();
		//	openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
		//	if (openFileDialog.ShowDialog() == true)
		//		ViewModel.ExportToCSV(openFileDialog.FileName);
		//}

		private void Import_Live_Registry_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ImportFromRegistry();
        }

        private void Import_Offline_Registry_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { ValidateNames = false, ReadOnlyChecked = true };
            if (openFileDialog.ShowDialog() == true)
                ViewModel.ImportFromRegistry(openFileDialog.FileName);
        }

        private void ResetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to reset the application (progress will be lost)?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                ViewModel.RestartApplication(isElevated);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch s && s.IsOn)
                (Application.Current as App).ChangeTheme(new Uri(@"UI/Themes/DarkTheme.xaml", UriKind.Relative));
            else
                (Application.Current as App).ChangeTheme(new Uri(@"UI/Themes/LightTheme.xaml", UriKind.Relative));
        }
    }
}
