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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeeShellsV2.Modules
{
	public class RTFModule : IPdfModule
	{
		public string Name => "RTFModule";
		private string Text { get; set; }
		public RichTextBox Rtb { get; set; }

		public ToggleButton btnBold { get; set; }
		public ToggleButton btnItalic { get; set; }
		public ToggleButton btnUnderline { get; set; }
		public ComboBox cmbFontFamily { get; set; }
		public ComboBox cmbFontSize { get; set; }

		public RTFModule()
		{
			Rtb = new RichTextBox();
			Rtb.Height = 50;
			Rtb.SelectionChanged += Rtb_SelectionChanged;

			btnBold = new ToggleButton();
			btnItalic = new ToggleButton();
			btnUnderline = new ToggleButton();
			cmbFontFamily = new ComboBox();
			cmbFontSize = new ComboBox();

			btnBold.Command = EditingCommands.ToggleBold;
			//Image bold = new Image();
			//BitmapImage bi = new BitmapImage();
			//bi.BeginInit();
			//bi.UriSource = new Uri("/SeeShellsV2;UI/Images/SeeShells.png", UriKind.RelativeOrAbsolute);
			//bi.EndInit();
			//bold.Width = 16;
			//bold.Height = 16;
			//bold.Source = bi;

			btnItalic.Command = EditingCommands.ToggleItalic;
			btnUnderline.Command = EditingCommands.ToggleUnderline;

			cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			cmbFontFamily.Width = 150;
			cmbFontFamily.SelectionChanged += CmbFontFamily_SelectionChanged;


			cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
			cmbFontSize.Width = 50;
			cmbFontSize.IsEditable = true;
			cmbFontSize.SelectionChanged += CmbFontSize_SelectionChanged;
		}

		public void Clone()
		{
			throw new NotImplementedException();
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
			StackPanel sp = new StackPanel();

			ToolBar tool = new ToolBar();

			tool.Items.Add(btnBold);
			tool.Items.Add(btnItalic);
			tool.Items.Add(btnUnderline);
			tool.Items.Add(cmbFontFamily);
			tool.Items.Add(cmbFontSize);

			sp.Children.Add(tool);
			sp.Children.Add(Rtb);

			string s = System.Windows.Markup.XamlWriter.Save(sp);
			FrameworkElement e = System.Xaml.XamlServices.Parse(s) as FrameworkElement;
			e.DataContext = this;
			return e;
		}

		private void Rtb_SelectionChanged(object sender, RoutedEventArgs e)
		{
			object temp = Rtb.Selection.GetPropertyValue(Inline.FontWeightProperty);
			btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
			temp = Rtb.Selection.GetPropertyValue(Inline.FontStyleProperty);
			btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
			temp = Rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
			btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

			temp = Rtb.Selection.GetPropertyValue(Inline.FontFamilyProperty);
			cmbFontFamily.SelectedItem = temp;
			temp = Rtb.Selection.GetPropertyValue(Inline.FontSizeProperty);
			cmbFontSize.Text = temp.ToString();
		}

		private void CmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Rtb.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
		}

		private void CmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbFontFamily.SelectedItem != null)
				Rtb.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
		}
	}
}
