using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SeeShellsV2.Repositories;
using SeeShellsV2.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Unity;

namespace SeeShellsV2.Services
{
	class OverviewModule : IPdfModule
	{
		// Events type donut chart
		// Shellbag count
		// Shellbag Event count
		// Events Span
		// Begin
		//	 -
		//	End
		public string Name => "Overview";

		public PlotView PiePlot { get; set; }

		[Dependency]
		public IShellEventCollection ShellEvents { get; set; }

		public PlotModel PieModel { get; set; }
		public ICollectionView FilteredShellEvents => ShellEvents.FilteredView;

		public OverviewModule([Dependency] IShellEventCollection shellEvents)
		{
			ShellEvents = shellEvents;

			PieModel = new PlotModel { Title = "Pie Sample1" };

			OxyPlot.Series.PieSeries seriesP1 = new OxyPlot.Series.PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0, InnerDiameter = 0.4 };
			

			seriesP1.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
			seriesP1.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
			seriesP1.Slices.Add(new PieSlice("Asia", 4157) { IsExploded = true });
			seriesP1.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
			seriesP1.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });

			PieModel.Series.Add(seriesP1);

		}

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public System.Windows.UIElement Render()
		{
			if (PiePlot == null)
				return null;
			var plot = PiePlot;
			var s = plot.ToBitmap();
			Image image = new Image();
			image.Source = s;
			image.Width = s.Width;
			image.Height = s.Height;

			StackPanel sp = new StackPanel();

			return image;
		}

		public FrameworkElement View()
		{

			string view = @"
				<StackPanel Orientation=""Horizontal"" Background=""White"">
						<oxy:PlotView Name=""PieSeries"" Height=""300"" Width=""300"" Model=""{Binding PieModel}""/>
						<StackPanel Grid.Column=""1"" >
							<TextBlock Text=""Number of ShellBags"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
							<TextBlock Text=""20"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
						</StackPanel>
						<StackPanel Grid.Column=""2"" >
							<TextBlock Text=""Number of ShellEvents"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
							<TextBlock Text=""20"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
						</StackPanel>
						<StackPanel Grid.Column=""3"" >
							<TextBlock Text=""Event Span"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
							<TextBlock Text=""Begin"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
							<TextBlock Text=""-"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
							<TextBlock Text=""End"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" />
						</StackPanel>
				</StackPanel>";


			//// add WPF namespaces to a parser context so we can parse WPF tags like StackPanel
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

			e.DataContext = this;

			var ps = e.FindName("PieSeries") as PlotView;
			PiePlot = ps;

			return e;
		}
	}
}
