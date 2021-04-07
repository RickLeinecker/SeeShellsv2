using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeeShellsV2.Modules
{
	public class RTFModule : IPdfModule
	{
		public string Name => "RTFModule";

		public FontFamily SelectedFontFamily { get; set; }
		public double SelectedFontSize { get; set; }

		public bool IsBold { get; set; }
		public bool IsItalic { get; set; }
		public bool IsUnderline { get; set; }

		public IEnumerable<FontFamily> FontFamilies => Fonts.SystemFontFamilies.OrderBy(f => f.Source);
		public IEnumerable<double> FontSizes => new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

		private RichTextBox Rtb { get; set; }

		public RTFModule()
		{
			// default font properties
			SelectedFontFamily = new FontFamily("Arial");
			SelectedFontSize = 12;
			IsBold = IsItalic = IsUnderline = false;
		}

		public IPdfModule Clone()
		{
			// we only really need a basic copy here to let the exporter
			// use this object as a template to make more objects
			return MemberwiseClone() as IPdfModule;
		}

		public void Render(PdfDocument doc)
		{
			//PdfPage page = doc.AddPage();
			//XGraphics gfx = XGraphics.FromPdfPage(page);
			//// HACK²
			//gfx.MUH = PdfFontEncoding.Unicode;
			//Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			//XFont font = new XFont("Verdana", 13, XFontStyle.Bold);

			//gfx.DrawString("The following paragraph was rendered using MigraDoc:", font, XBrushes.Black,
			//  new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);

			//// You always need a MigraDoc document for rendering.
			//Document docu = new Document();
			//MigraDoc.DocumentObjectModel.Section sec = docu.AddSection();
			//// Add a single paragraph with some text and format information.
			//Paragraph para = sec.AddParagraph();
			//para.Format.Alignment = ParagraphAlignment.Justify;
			//para.Format.Font.Name = "Times New Roman";
			//para.Format.Font.Size = 12;
			//para.Format.Font.Color = Colors.DarkGray;
			//para.Format.Font.Color = Colors.DarkGray;
			//para.AddText("Duisism odigna acipsum delesenisl ");
			//para.AddFormattedText("ullum in velenit", TextFormat.Bold);
			//para.AddText(" ipit iurero dolum zzriliquisis nit wis dolore vel et nonsequipit, velendigna " +
			//  "auguercilit lor se dipisl duismod tatem zzrit at laore magna feummod oloborting ea con vel " +
			//  "essit augiati onsequat luptat nos diatum vel ullum illummy nonsent nit ipis et nonsequis " +
			//  "niation utpat. Odolobor augait et non etueril landre min ut ulla feugiam commodo lortie ex " +
			//  "essent augait el ing eumsan hendre feugait prat augiatem amconul laoreet. ≤≥≈≠");
			//para.Format.Borders.Distance = "5pt";
			//para.Format.Borders.Color = Colors.Gold;

			//// Create a renderer and prepare (=layout) the document
			//DocumentRenderer docRenderer = new DocumentRenderer(docu);
			//docRenderer.PrepareDocument();

			//// Render the paragraph. You can render tables or shapes the same way.
			//docRenderer.RenderObject(gfx, XUnit.FromCentimeter(5), XUnit.FromCentimeter(10), "12cm", para);
		}

		public FrameworkElement View()
		{
			// the view we will display for the user to configure this object
			string view = @"
			<StackPanel>
				<ToolBar>
					<ToggleButton Name=""BoldBtn"" IsChecked=""{Binding IsBold}""
                                  Command=""EditingCommands.ToggleBold"" />
					<ToggleButton Name=""ItalicBtn"" IsChecked=""{Binding IsItalic}""
                                  Command=""EditingCommands.ToggleItalic"" />
					<ToggleButton Name=""UnderlineBtn"" IsChecked=""{Binding IsUnderline}""
                                  Command=""EditingCommands.ToggleUnderline"" />
					<ComboBox Name=""FontFamilyCmb""
                              SelectedItem=""{Binding SelectedFontFamily}""
                              ItemsSource=""{Binding FontFamilies}"" Width=""150"" />
					<ComboBox Name=""FontSizeCmb""
                              SelectedItem=""{Binding SelectedFontSize}""
                              ItemsSource=""{Binding FontSizes}""
                              Width=""50"" IsEditable=""True"" />
				</ToolBar>
				<RichTextBox Name=""RichTextBox"" Height=""50"" />
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
			// **any data access that can be done with xaml bindings should be done with xaml bindings**
			var rtb = e.FindName("RichTextBox") as RichTextBox;
			var btnBold = e.FindName("BoldBtn") as ToggleButton;
			var btnItalic = e.FindName("ItalicBtn") as ToggleButton;
			var btnUnderline = e.FindName("UnderlineBtn") as ToggleButton;
			var cmbFontFamily = e.FindName("FontFamilyCmb") as ComboBox;
			var cmbFontSize = e.FindName("FontSizeCmb") as ComboBox;

			// hook up event listeners
			rtb.SelectionChanged += Rtb_SelectionChanged;
			cmbFontSize.SelectionChanged += CmbFontSize_SelectionChanged;
			cmbFontFamily.SelectionChanged += CmbFontFamily_SelectionChanged;

			// save RTB element for later
			Rtb = rtb;

			return e;
		}

		private void Rtb_SelectionChanged(object sender, RoutedEventArgs e)
		{
			object temp = Rtb.Selection.GetPropertyValue(Inline.FontWeightProperty);
			IsBold = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
			temp = Rtb.Selection.GetPropertyValue(Inline.FontStyleProperty);
			IsItalic = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
			temp = Rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
			IsUnderline = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

			temp = Rtb.Selection.GetPropertyValue(Inline.FontFamilyProperty);
			if (temp is FontFamily f)
				SelectedFontFamily = f;
			temp = Rtb.Selection.GetPropertyValue(Inline.FontSizeProperty);
			if (temp is double size)
				SelectedFontSize = size;
		}

		private void CmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Rtb.Selection.ApplyPropertyValue(Inline.FontSizeProperty, SelectedFontSize);
		}

		private void CmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SelectedFontFamily != null)
				Rtb.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, SelectedFontFamily);
		}
	}
}
