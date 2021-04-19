using SeeShellsV2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Unity;

namespace SeeShellsV2.Services
{
	class TimeHistoModule : IPdfModule
	{
		public string Name => "Timeline Histogram";

		public FrameworkElement TimeHisto { get; set; }

		[Dependency]
		public ITimelineViewAltVM vm { get; set; }

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			throw new NotImplementedException();
		}

		public FrameworkElement View()
		{
			string view = @"
			<Grid>
			<local:TimeSeriesHistogram x:Name=""Histogram"" Height=""400"" Width=""500"" Background=""White""
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

			//// construct the view using an XAML parser
			FrameworkElement e = XamlReader.Parse(view, context) as FrameworkElement;

			e.DataContext = vm;

			var th = e.FindName("Histogram") as TimeSeriesHistogram;

			TimeHisto = th;

			return e;
		}
	}
}
