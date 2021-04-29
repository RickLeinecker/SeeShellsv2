using SeeShellsV2.UI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Unity;

namespace SeeShellsV2.Services
{
	class CaptionedTimeHistoModule : IPdfModule
	{
		public string Name => "Captioned Histogram";
		public FrameworkElement TimeHisto { get; set; }
		public FrameworkElement TextBox { get; set; }

		[Dependency]
		public ITimelineViewVM vm { get; set; }

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			if (TextBox == null || TimeHisto == null)
				return null;

			TextBlock caption = new TextBlock();
			caption.TextWrapping = TextWrapping.Wrap;
			caption.TextAlignment = TextAlignment.Left;
			caption.Width = 350;
			caption.Height = 900;
			caption.Text = (TextBox as TextBox).Text;
			caption.FontFamily = (TextBox as TextBox).FontFamily;
			caption.FontSize = (TextBox as TextBox).FontSize;
			caption.FontWeight = (TextBox as TextBox).FontWeight;

			var plot = (TimeHisto as TimeSeriesHistogram).HistogramPlot;
			var s = plot.ToBitmap();
			Image image = new Image();
			image.Source = s;
			image.Width = s.Width;
			image.Height = s.Height;

			StackPanel captioned = new StackPanel();
			captioned.Orientation = Orientation.Horizontal;
			captioned.Children.Add(image);
			captioned.Children.Add(caption);

			return captioned;
		}

		public FrameworkElement View()
		{
			string view = @"
			<Grid>
				<Grid.ColumnDefinitions>
						<ColumnDefinition Width="" * ""/>
						<ColumnDefinition Width="" * ""/>
					</Grid.ColumnDefinitions >
					<Grid.RowDefinitions >
						<RowDefinition Height = ""800""/>
					</Grid.RowDefinitions >
				<local:TimeSeriesHistogram x:Name=""Histogram"" Background=""White"" Height=""500"" Grid.Column=""0""
					ColorProperty = ""{Binding ColorProperty, Mode=OneWay}""
					YAxisTitle = ""User Action Frequency""
					MinimumDate = ""{Binding DateSelectionBegin}""
					MaximumDate = ""{Binding DateSelectionEnd}""
					ItemsSource = ""{Binding ShellEvents}"" DateTimeProperty = ""TimeStamp""/>
				<StackPanel Grid.Column=""1"">
				<ToolBar>
					<Button Command=""ApplicationCommands.Cut"" ToolTip=""Cut"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C6;""/>
					<Button Command=""ApplicationCommands.Copy"" ToolTip= ""Copy"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C8;""/>
					<Button Command=""ApplicationCommands.Paste"" ToolTip= ""Paste"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE77F;""/>	   
				</ToolBar>
				<TextBox Name=""TextBox""
					TextAlignment=""Left""
					TextWrapping=""Wrap"" 
					BorderBrush=""Transparent"" CaretBrush=""Black"" 
					Foreground=""Black"" Background=""White"" 
					AcceptsTab=""True"" AcceptsReturn=""True""
					Height=""700"" Width=""300"" 
					MaxHeight=""700"" MaxLength=""2216""/>
				</StackPanel>
			</Grid>";

			ParserContext context = new ParserContext();
			context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
			context.XmlnsDictionary.Add("d", "http://schemas.microsoft.com/expression/blend/2008");
			context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			context.XmlnsDictionary.Add("oxy", "http://oxyplot.org/wpf");
			context.XmlnsDictionary.Add("mah", "http://metro.mahapps.com/winfx/xaml/controls");

			CalendarHeatMap heat = new CalendarHeatMap();
			Type type = heat.GetType();
			context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
			context.XamlTypeMapper.AddMappingProcessingInstruction("local", type.Namespace, type.Assembly.FullName);
			context.XmlnsDictionary.Add("local", "local");

			// construct the view using an XAML parser
			FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

			e.DataContext = vm;

			var th = e.FindName("Histogram") as TimeSeriesHistogram;
			var tb = e.FindName("TextBox") as TextBox;

			TimeHisto = th;
			TextBox = tb;

			return e;
		}
	}
}
