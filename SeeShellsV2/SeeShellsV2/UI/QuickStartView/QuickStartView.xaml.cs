using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SeeShellsV2.UI
{
    /// <summary>
    /// Interaction logic for QuickStartView.xaml
    /// </summary>
    public partial class QuickStartView : UserControl
    {
        public IMainWindowVM ViewModel
        {
            get => DataContext as IMainWindowVM;
        }

        public QuickStartView()
        {
            InitializeComponent();

            Random rand = new Random();

            if (rand.Next(100) == 42)
                Subtitle.Text = "Another Day in Paradise.";
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Border b && b.Name == "OutsideBorder")
                Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Name)
                {
                    case "ImportActiveRegistry":
                        if (ViewModel.ImportFromRegistry())
                            Visibility = Visibility.Hidden;
                        break;
                    case "ImportRegistryFile":
                        OpenFileDialog openFileDialog = new OpenFileDialog { ValidateNames = false, ReadOnlyChecked = true };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            Visibility = Visibility.Hidden;
                            ViewModel.ImportFromRegistry(openFileDialog.FileName);
                        }
                        break;
                    case "GotoWebsite":
                        OpenPage(ViewModel.WebsiteUrl);
                        break;
                    case "GotoGithub":
                        OpenPage(ViewModel.GithubUrl);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void OpenPage(string url)
        {
            Process browser = new Process();
            browser.StartInfo.UseShellExecute = true;
            browser.StartInfo.FileName = url;
            browser.Start();
        }
    }
}
