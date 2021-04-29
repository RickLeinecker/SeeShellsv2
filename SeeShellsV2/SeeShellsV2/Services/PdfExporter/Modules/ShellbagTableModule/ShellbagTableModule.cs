using SeeShellsV2.Repositories;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Unity;

namespace SeeShellsV2.Services
{
	class ShellbagTableModule : IPdfModule
	{
		public string Name => "Filtered Shellbag Event Table";

        private FrameworkElement Datagrid { get; set; }

        [Dependency]
        public IShellEventCollection ShellEvents { get; set; }
        public ICollectionView FilteredShellEvents => ShellEvents.FilteredView;

        public ShellbagTableModule([Dependency] IShellEventCollection shellEvents)
        {
            ShellEvents = shellEvents;
        }

        public IPdfModule Clone()
		{
            return MemberwiseClone() as IPdfModule;
        }

		public UIElement Render()
		{
            return Datagrid as UIElement;
        }

		public FrameworkElement View()
		{
            string view = @"
                <Grid MaxHeight=""800"" Width=""800"">
                    <DataGrid Name=""Data"" ItemsSource = ""{Binding FilteredShellEvents}""
                            AutoGenerateColumns = ""False""
                            CanUserAddRows = ""False"" IsReadOnly = ""True"" Height=""Auto"">
                        <DataGrid.Columns>
                            <DataGridTextColumn Header = ""Event Time"" Binding = ""{Binding TimeStamp}"" />
                            <DataGridTextColumn Header = ""Location Name"" Binding = ""{Binding Place.Name}"" />
                            <DataGridTextColumn Header = ""Type"" Binding = ""{Binding TypeName}"" />
                            <DataGridTextColumn Header = ""User"" Binding = ""{Binding User.Name}"" />
                        </DataGrid.Columns>
                    </DataGrid>
                </Grid>";

            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
            context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");

            // construct the view using an XAML parser
            FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

            // assign this object as the data context so the view can get/set module properties
            e.DataContext = this;

            // extract the elements from the view. this is only necessary if we
            // want to use these elements *directly* to do work inside this class.
            var data = e.FindName("Data") as DataGrid;

            // save RTB element for later
            Datagrid = data;

            return e;
        }
	}
}
