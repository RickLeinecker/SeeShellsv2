using SeeShellsV2.Repositories;
using SeeShellsV2.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using Unity;

namespace SeeShellsV2.Services
{
	class CaptionedHeatMapModule : IPdfModule
	{
		public string Name => "Captioned HeatMap";

		public FrameworkElement HeatMap { get; set; }
		public FrameworkElement Rtb { get; set; }

		[Dependency]
		public IShellEventCollection ShellEvents { get; set; }


		public ICollectionView FilteredShellEvents => ShellEvents.FilteredView;

		public CaptionedHeatMapModule([Dependency] IShellEventCollection shellEvents)
		{
			ShellEvents = shellEvents;
		}

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			if (Rtb == null || HeatMap == null)
				return null;

			string s = XamlWriter.Save(Rtb);
			StringReader sr = new StringReader(s);
			XmlReader reader = XmlTextReader.Create(sr, new XmlReaderSettings());
			FrameworkElement e = (FrameworkElement)XamlReader.Load(reader);
			var rtb = e.FindName("RichTextBox") as RichTextBox;

			var plot = (HeatMap as CalendarHeatMap).HeatMapPlot;
			var bmp = plot.ToBitmap();
			Image image = new Image();
			image.Source = bmp;
			image.Width = bmp.Width;
			image.Height = bmp.Height;

			StackPanel sp = new StackPanel();
			sp.Width = image.Width;
			TextBlock t = new TextBlock();
			t.Text = (HeatMap as CalendarHeatMap).Year.ToString();
			t.FontSize = plot.TitleFontSize;
			t.FontWeight = plot.TitleFontWeight;
			t.FontFamily = plot.FontFamily;
			t.HorizontalAlignment = HorizontalAlignment.Center;
			sp.Children.Add(t);
			sp.Children.Add(image);

			BlockUIContainer bc = new BlockUIContainer();
			bc.Child = sp;

			Figure fig = new Figure();
			fig.HorizontalAnchor = FigureHorizontalAnchor.PageLeft;
			//FigureLength fl = new FigureLength(0.3, FigureUnitType.Column);
			//fig.Width = fl;
			fig.Blocks.Add(bc);

			Paragraph p = new Paragraph();
			p.Inlines.Add(fig);

			//Floater fig = new Floater();
			//fig.HorizontalAlignment = HorizontalAlignment.Left;
			//fig.Width = image.Width;
			//fig.Blocks.Add(bc);

			List<Block> rtbBlocks = new List<Block>(rtb.Document.Blocks);

			foreach (Block block in rtbBlocks)
			{
				Debug.WriteLine(block.GetType().Name);
			}

			foreach (Block block in rtbBlocks)
			{
				List<Inline> inlines = new List<Inline>((block as Paragraph).Inlines);
				p.Inlines.AddRange(inlines);
			}

			FlowDocument fd = new FlowDocument(p);
			RichTextBox r = new RichTextBox(fd);
			//StackPanel captioned = new StackPanel();
			//Grid captioned = new Grid();
			//captioned.ColumnDefinitions.Add(new ColumnDefinition());
			//captioned.ColumnDefinitions.Add(new ColumnDefinition());
			//captioned.RowDefinitions.Add(new RowDefinition());
			//RowDefinition r = new RowDefinition();
			//r.Height = GridLength.Auto;
			//captioned.RowDefinitions.Add(r);
			//Grid.SetColumn(sp, 0);
			//Grid.SetRow(sp, 0);
			//Grid.SetColumn(rtb, 1);
			//Grid.SetRowSpan(rtb, 2);
			//captioned.Children.Add(sp);
			//captioned.Children.Add(rtb);

			return r as UIElement;
		}

		public FrameworkElement View()
		{

			// the view we will display for the user to configure this object
			string view = @"
			<StackPanel Orientation=""Horizontal"" MinHeight=""700"">
				<local:CalendarHeatMap x:Name=""Heatmap""
					Background=""White""
					ColorAxisTitle =""User Action Frequency""
					ItemsSource =""{Binding FilteredShellEvents}""
					SelectionBegin=""{Binding DateSelectionBegin}""
					SelectionEnd=""{Binding DateSelectionEnd}""
					SelectionColor=""{ DynamicResource MahApps.Colors.Accent}""
					DateTimeProperty=""TimeStamp"" Orientation=""Vertical"" />
				<StackPanel>
				<ToolBar>
					<Button Command=""ApplicationCommands.Cut"" ToolTip=""Cut"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C6;""/>
					<Button Command=""ApplicationCommands.Copy"" ToolTip= ""Copy"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C8;""/>
					<Button Command=""ApplicationCommands.Paste"" ToolTip= ""Paste"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE77F;""/>
					<ToggleButton Command= ""EditingCommands.ToggleBold"" ToolTip= ""Bold"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DD;""/>
					<ToggleButton Command= ""EditingCommands.ToggleItalic"" ToolTip= ""Italic""  FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DB;""/>
					<ToggleButton Command= ""EditingCommands.ToggleUnderline"" ToolTip= ""Underline"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DC;""/>
					<Button Command= ""EditingCommands.IncreaseFontSize"" ToolTip= ""Grow Font"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E8;""/>
					<Button Command= ""EditingCommands.DecreaseFontSize"" ToolTip= ""Shrink Font"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E7;""/>
					<Button Command= ""EditingCommands.ToggleBullets"" ToolTip= ""Bullets"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8FD;""/>
					<Button Command= ""EditingCommands.AlignLeft"" ToolTip= ""Align Left"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E4;"" />
					<Button Command= ""EditingCommands.AlignCenter"" ToolTip= ""Align Center"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E3;""/>
					<Button Command= ""EditingCommands.AlignRight"" ToolTip= ""Align Right"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E2;""/>
					<Button Command= ""EditingCommands.AlignJustify"" ToolTip= ""Align Justify"">
						<Grid>
							<TextBlock FontFamily=""Segoe MDL2 Assets"" Text=""&#xE8E4;"" VerticalAlignment=""Center"" TextAlignment=""Center""/>
							<TextBlock FontFamily=""Segoe MDL2 Assets"" Text=""&#xE8E2;"" VerticalAlignment=""Center"" TextAlignment=""Center""/>
						</Grid>
					</Button>			   
				</ToolBar>
				<RichTextBox Name=""RichTextBox"" BorderBrush=""Transparent"" CaretBrush=""Black"" Foreground=""Black"" Background=""White"" AcceptsTab=""True"" Height=""700"" MaxHeight=""700""/>
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

			//// assign this object as the data context so the view can get/set module properties
			e.DataContext = this;

			//// extract the elements from the view. this is only necessary if we
			//// want to use these elements *directly* to do work inside this class.
			//// **any data access that can be done with xaml bindings should be done with xaml bindings**
			var hm = e.FindName("Heatmap") as CalendarHeatMap;
			var rtb = e.FindName("RichTextBox") as RichTextBox;

			//// hook up event listeners

			//// save RTB element for later
			HeatMap = hm;
			Rtb = rtb;

			return e;
		}
	}
}
