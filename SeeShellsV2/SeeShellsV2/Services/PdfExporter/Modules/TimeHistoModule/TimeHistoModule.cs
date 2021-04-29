using SeeShellsV2.UI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Unity;

namespace SeeShellsV2.Services
{
	class TimeHistoModule : IPdfModule
	{
		public string Name => "Timeline Histogram";

		public FrameworkElement TimeHisto { get; set; }

		[Dependency]
		public ITimelineViewVM vm { get; set; }

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			if (TimeHisto == null)
				return null;

			var plot = (TimeHisto as TimeSeriesHistogram).HistogramPlot;
			var s = plot.ToBitmap();
			Image image = new Image();
			image.Source = s;
			image.Width = s.Width;
			image.Height = s.Height;

			return image;
		}

		public FrameworkElement View()
		{
			string view = @"
			<Grid>
			<local:TimeSeriesHistogram x:Name=""Histogram"" Background=""White"" Height=""500""
										   ColorProperty = ""{Binding ColorProperty, Mode=OneWay}""
										   YAxisTitle = ""User Action Frequency""
										   MinimumDate = ""{Binding DateSelectionBegin}""
										   MaximumDate = ""{Binding DateSelectionEnd}""
										   ItemsSource = ""{Binding ShellEvents}"" DateTimeProperty = ""TimeStamp""/>
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

			FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

			e.DataContext = vm;

			var th = e.FindName("Histogram") as TimeSeriesHistogram;

			TimeHisto = th;

			return e;
		}
	}
}
