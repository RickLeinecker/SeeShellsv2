﻿using System;
using System.IO;
using System.Windows;
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
        public void ImportFromOnlineRegistry();
        public void ImportFromOfflineRegistry(string hiveLocation);
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

        private void Import_Live_Registry_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ImportFromOnlineRegistry();
        }

        private void Import_Offline_Registry_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.ReadOnlyChecked = true;
            if (openFileDialog.ShowDialog() == true)
                ViewModel.ImportFromOfflineRegistry(openFileDialog.FileName);
        }
    }
}