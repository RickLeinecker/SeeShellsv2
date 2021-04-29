using System;
using SeeShellsV2.Repositories;
using SeeShellsV2.UI;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;
using Unity;

namespace SeeShellsV2.Services
{
	class ChartsModule : IPdfModule
	{
		public string Name => "HeatMap and Histogram";

		public FrameworkElement HeatMap { get; set; }
		public FrameworkElement TimeHisto { get; set; }

		[Dependency]
		public ITimelineViewVM vm { get; set; }

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			if (HeatMap == null)
				return null;
			var hmplot = (HeatMap as CalendarHeatMap).HeatMapPlot;
			var hms = hmplot.ToBitmap();
			Image hmImage = new Image();
			hmImage.Source = hms;
			hmImage.Width = hms.Width;
			hmImage.Height = hms.Height;

			StackPanel sp = new StackPanel();
			TextBlock t = new TextBlock();
			t.Text = (HeatMap as CalendarHeatMap).Year.ToString();
			t.FontSize = hmplot.TitleFontSize;
			t.FontWeight = hmplot.TitleFontWeight;
			t.FontFamily = hmplot.FontFamily;
			t.HorizontalAlignment = HorizontalAlignment.Center;
			sp.Children.Add(t);
			sp.Children.Add(hmImage);

			var tplot = (TimeHisto as TimeSeriesHistogram).HistogramPlot;
			var ts = tplot.ToBitmap();
			Image tImage = new Image();
			tImage.Source = ts;
			tImage.Width = ts.Width;
			tImage.Height = ts.Height;

			StackPanel s = new StackPanel();
			s.Orientation = Orientation.Horizontal;
			s.Children.Add(sp);
			s.Children.Add(tImage);

			return s;
		}

		public FrameworkElement View()
		{
			string view = @"
				<Grid>
					<Grid.ColumnDefinitions>
						<ColumnDefinition Width="" * ""/>
						<ColumnDefinition Width="" 3* ""/>
					</Grid.ColumnDefinitions >
					<Grid.RowDefinitions >
						<RowDefinition Height = ""700""/>
					</Grid.RowDefinitions >
						<local:CalendarHeatMap x:Name = ""Heatmap"" Grid.Column=""0""
							Background=""White""
							ColorAxisTitle = ""User Action Frequency""
							ItemsSource = ""{Binding FilteredShellEvents}""
							SelectionBegin = ""{Binding DateSelectionBegin}""
							SelectionEnd = ""{Binding DateSelectionEnd}""
							SelectionColor=""{ DynamicResource MahApps.Colors.Accent}""
							DateTimeProperty = ""TimeStamp"" Orientation = ""Horizontal"" />
						<local:TimeSeriesHistogram x:Name=""Histogram"" Background=""White"" Grid.Column=""1""
							ColorProperty = ""{Binding ColorProperty, Mode=OneWay}""
							YAxisTitle = ""User Action Frequency""
							MinimumDate = ""{Binding DateSelectionBegin}""
							MaximumDate = ""{Binding DateSelectionEnd}""
							ItemsSource = ""{Binding ShellEvents}"" DateTimeProperty = ""TimeStamp""/>
				</Grid>";


			// add WPF namespaces to a parser context so we can parse WPF tags like StackPanel
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

			// assign this object as the data context so the view can get/set module properties
			e.DataContext = vm;

			// extract the elements from the view. this is only necessary if we
			// want to use these elements *directly* to do work inside this class.
			var hm = e.FindName("Heatmap") as CalendarHeatMap;
			var th = e.FindName("Histogram") as TimeSeriesHistogram;

			// save elements for later
			TimeHisto = th;
			HeatMap = hm;

			return e;
		}
	}
}
