using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace SeeShellsV2.Services
{
	class HeaderModule : IPdfModule
	{
		public string Name => "Header";
		private FrameworkElement Header { get; set; }

		public IPdfModule Clone()
		{
			return MemberwiseClone() as IPdfModule;
		}

		public UIElement Render()
		{
			if (Header == null)
				return null;
			string s = XamlWriter.Save(Header);
			StringReader sr = new StringReader(s);
			XmlReader reader = XmlTextReader.Create(sr, new XmlReaderSettings());
			FrameworkElement e = (FrameworkElement)XamlReader.Load(reader);
			return e.FindName("RichTextBox") as UIElement;
		}

		public FrameworkElement View()
		{
			string view = @"
			<StackPanel>
				<ToolBar>
					<Button Command=""ApplicationCommands.Cut"" ToolTip=""Cut"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C6;""/>
					<Button Command = ""ApplicationCommands.Copy"" ToolTip = ""Copy"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8C8;""/>
					<Button Command = ""ApplicationCommands.Paste"" ToolTip = ""Paste"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE77F;""/>
					<ToggleButton Command = ""EditingCommands.ToggleBold"" ToolTip = ""Bold"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DD;""/>
					<ToggleButton Command = ""EditingCommands.ToggleItalic"" ToolTip = ""Italic""  FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DB;""/>
					<ToggleButton Command = ""EditingCommands.ToggleUnderline"" ToolTip = ""Underline"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8DC;""/>
					<Button Command = ""EditingCommands.IncreaseFontSize"" ToolTip = ""Grow Font"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E8;""/>
					<Button Command = ""EditingCommands.DecreaseFontSize"" ToolTip = ""Shrink Font"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E7;""/>
					<Button Command = ""EditingCommands.ToggleBullets"" ToolTip = ""Bullets"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8FD;""/>
					<Button Command = ""EditingCommands.AlignLeft"" ToolTip = ""Align Left"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E4;"" />
					<Button Command = ""EditingCommands.AlignCenter"" ToolTip = ""Align Center"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E3;""/>
					<Button Command = ""EditingCommands.AlignRight"" ToolTip = ""Align Right"" FontFamily=""Segoe MDL2 Assets"" Content=""&#xE8E2;""/>
					<Button Command = ""EditingCommands.AlignJustify"" ToolTip = ""Align Justify"">
						<Grid>
							<TextBlock FontFamily=""Segoe MDL2 Assets"" Text=""&#xE8E4;"" VerticalAlignment=""Center"" TextAlignment=""Center""/>
							<TextBlock FontFamily=""Segoe MDL2 Assets"" Text=""&#xE8E2;"" VerticalAlignment=""Center"" TextAlignment=""Center""/>
						</Grid>
					</Button>			   
				</ToolBar>
				<RichTextBox Name=""RichTextBox"" BorderBrush=""Transparent"" CaretBrush=""Black"" Foreground=""Black"" Background=""White"" AcceptsTab=""True"">
					<FlowDocument>
						<Paragraph FontSize=""20"" TextAlignment=""Center""><Bold>SeeShells Report</Bold>
						</Paragraph>
						<Paragraph FontSize=""16"" TextAlignment=""Center"">A Shellbag Forensics Tool</Paragraph>  
					</FlowDocument>
				</RichTextBox>
			</StackPanel>";

			// add WPF namespaces to a parser context so we can parse WPF tags like StackPanel
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
			var rtb = e.FindName("RichTextBox") as RichTextBox;

			// hook up event listeners
			// Image Source = "".. / .. / .. / .. / UI / Images / SeeShells.png""
			// save RTB element for later
			Header = rtb;

			return e;
		}
	}
}
