using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.UI;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using Unity;

namespace SeeShellsV2.Services
{
	class OverviewModule : IPdfModule
	{
		public string Name => "Overview";

		public PlotView PiePlot { get; set; }
		public FrameworkElement SBCount { get; set; }
		public FrameworkElement SECount { get; set; }
		public FrameworkElement ESpan { get; set; }

		[Dependency]
		public IShellEventCollection ShellEvents { get; set; }

		public int ShellbagsCount{ get; set; }
		public int EventsCount { get; set; }
		public string BeginDate { get; set; }
		public string EndDate { get; set; }


		public PlotModel PieModel { get; set; }
		public ICollectionView FilteredShellEvents => ShellEvents.FilteredView;

		public OverviewModule([Dependency] IShellEventCollection shellEvents, [Dependency] IShellItemCollection shellItems)
		{
				
			ShellEvents = shellEvents;

			PieModel = new PlotModel { Title = "ShellEvents" };

			OxyPlot.Series.PieSeries seriesP1 = new OxyPlot.Series.PieSeries {FontSize=12, ExplodedDistance = 0.1, StrokeThickness = 2.0, InsideLabelPosition = 0.6, AngleSpan = 360, StartAngle = 0, InnerDiameter = 0.4};

			int OtherCount = 0;

			foreach (string type in shellEvents.FilteredView.OfType<IShellEvent>().Select(e => e.GetType()).Distinct().Select(t => t.Name))
			{
				if (((double)shellEvents.FilteredView.OfType<IShellEvent>().Where(e => (e.GetType().Name == type) == true).Count() / shellEvents.FilteredView.OfType<IShellEvent>().Count()) * 100 > 5)
					seriesP1.Slices.Add(new PieSlice(type.Replace("Event",""), shellEvents.FilteredView.OfType<IShellEvent>().Where(e => (e.GetType().Name == type) == true).Count()) { IsExploded=true });
				else
					OtherCount += shellEvents.FilteredView.OfType<IShellEvent>().Where(e => (e.GetType().Name == type) == true).Count();
			}
			seriesP1.Slices.Add(new PieSlice("Other", OtherCount) { IsExploded = true });

			if (shellEvents.FilteredView.OfType<IShellEvent>().Any())
			{
				BeginDate = shellEvents.FilteredView.OfType<IShellEvent>().OrderBy(e => e.TimeStamp).First().TimeStamp.Date.ToString("MM-dd-yyyy");
				EndDate = shellEvents.FilteredView.OfType<IShellEvent>().OrderBy(e => e.TimeStamp).Last().TimeStamp.Date.ToString("MM-dd-yyyy");
			}

			ShellbagsCount = shellItems.Count;
			EventsCount = shellEvents.FilteredView.OfType<IShellEvent>().Count();

			PieModel.Series.Add(seriesP1);
		}

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public System.Windows.UIElement Render()
		{
			if (PiePlot == null || SBCount == null || SECount == null || ESpan == null)
				return null;
			var plot = PiePlot;
			var s = plot.ToBitmap();
			Image image = new Image();
			image.Source = s;
			image.Width = s.Width;
			image.Height = s.Height;

			string bagcount = XamlWriter.Save(SBCount);
			StringReader bc = new StringReader(bagcount);
			XmlReader readerbc = XmlTextReader.Create(bc, new XmlReaderSettings());
			FrameworkElement bcount = (FrameworkElement)XamlReader.Load(readerbc);
			StackPanel sbc= bcount.FindName("ShellbagCount") as StackPanel;

			string eventcount = XamlWriter.Save(SECount);
			StringReader ec = new StringReader(eventcount);
			XmlReader readerec = XmlTextReader.Create(ec, new XmlReaderSettings());
			FrameworkElement ecount = (FrameworkElement)XamlReader.Load(readerec);
			StackPanel sec = ecount.FindName("ShellEventCount") as StackPanel;

			string espan = XamlWriter.Save(ESpan);
			StringReader esp = new StringReader(espan);
			XmlReader readeres = XmlTextReader.Create(esp, new XmlReaderSettings());
			FrameworkElement evespan = (FrameworkElement)XamlReader.Load(readeres);
			StackPanel es = evespan.FindName("EventSpan") as StackPanel;


			Grid g = new Grid();
			ColumnDefinition iCol = new ColumnDefinition();
			iCol.Width = new GridLength(350);
			g.ColumnDefinitions.Add(iCol);
			g.ColumnDefinitions.Add(new ColumnDefinition());
			g.ColumnDefinitions.Add(new ColumnDefinition());
			g.ColumnDefinitions.Add(new ColumnDefinition());
			RowDefinition iRow = new RowDefinition();
			iRow.Height = new GridLength(325);
			g.RowDefinitions.Add(iRow);

			Grid.SetColumn(image, 0);
			Grid.SetRow(image, 0);

			Grid.SetColumn(sbc, 1);
			Grid.SetRow(sbc, 0);

			Grid.SetColumn(sec, 2);
			Grid.SetRow(sec, 0);
			Grid.SetColumn(es, 3);
			Grid.SetRow(es, 0);

			g.Children.Add(image);
			g.Children.Add(sbc);
			g.Children.Add(sec);
			g.Children.Add(es);

			return g;
		}

		public FrameworkElement View()
		{

			string view = @"
				<Grid Background=""White"">
					<Grid.ColumnDefinitions>
						<ColumnDefinition Width=""335""/>
						<ColumnDefinition Width="" * ""/>
						<ColumnDefinition Width="" * ""/>
						<ColumnDefinition Width="" * ""/>
					</Grid.ColumnDefinitions >
					<Grid.RowDefinitions >
						<RowDefinition Height = ""300""/>
					</Grid.RowDefinitions >
						<oxy:PlotView Grid.Column=""0"" Name=""PieSeries"" Height=""300"" Width=""335"" Model=""{Binding PieModel}""/>
						<StackPanel Name=""ShellbagCount"" Grid.Column=""1""  HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
							<TextBlock Text=""Number"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock Text=""of"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock Text=""Shellbags"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
							<TextBlock Text=""{Binding ShellbagsCount}"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""16"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
						</StackPanel>
						<StackPanel Name=""ShellEventCount"" Grid.Column=""2""  HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
							<TextBlock Text=""Number"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock Text=""of"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock Text=""ShellEvents"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
							<TextBlock Text=""{Binding EventsCount}"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" 
											FontSize=""16"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
						</StackPanel>
						<StackPanel Name=""EventSpan"" Grid.Column=""3""  HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
							<TextBlock Text=""Event Span"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" 
											FontSize=""18"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock  HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
							<TextBlock Text=""{Binding BeginDate}"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" 
											FontSize=""16"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock Text=""-"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
											FontSize=""16"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
							<TextBlock Text=""{Binding EndDate}"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" 
											FontSize=""16"" FontFamily=""Segoe UI"" FontWeight=""Bold""/>
						</StackPanel>
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

			e.DataContext = this;

			var ps = e.FindName("PieSeries") as PlotView;
			var sbc = e.FindName("ShellbagCount") as FrameworkElement;
			var sec = e.FindName("ShellEventCount") as FrameworkElement;
			var es = e.FindName("EventSpan") as FrameworkElement;

			PiePlot = ps;
			SBCount = sbc;
			SECount = sec;
			ESpan = es;

			return e;
		}
	}
}
