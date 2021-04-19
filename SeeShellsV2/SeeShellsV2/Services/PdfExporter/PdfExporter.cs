using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using Unity;
using System.Windows.Documents;
using System.Windows.Markup;

namespace SeeShellsV2.Services
{
	public class PdfExporter : IPdfExporter
	{
		public IEnumerable<IPdfModule> modules;
		public Dictionary<string, IPdfModule> moduleNames;
		public PdfExporter([Dependency] IUnityContainer container)
		{
			modules = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => typeof(IPdfModule).IsAssignableFrom(p))
				.Where(q => q.IsClass)
				.Select(r => (IPdfModule)container.Resolve(r));

			moduleNames = new Dictionary<string, IPdfModule>();
			foreach (IPdfModule module in modules)
			{
				moduleNames.Add(module.Name, module);
			}
		}

		public void Export(string filename, ObservableCollection<IPdfModule> moduleList)
		{

			PrintDialog pd = new PrintDialog();
			if ((pd.ShowDialog() == true))
			{
				FlowDocument fd = new FlowDocument();
				fd.PagePadding = new Thickness(50);
				fd.ColumnGap = 0;
				fd.ColumnWidth = pd.PrintableAreaWidth;
				foreach (IPdfModule module in moduleList)
				{
					BlockUIContainer bc = new BlockUIContainer();
					if (module.GetType().Name == "RTFModule")
					{
						List<Block> fdBlocks = new List<Block>((module.Render() as RichTextBox).Document.Blocks);
						foreach (Block block in fdBlocks)
						{
							fd.Blocks.Add(block);
						}
					}
					else 
					{
						bc.Child = module.Render();
						//Size sz = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
						//bc.Child.Measure(sz);
						//bc.Child.Arrange(new Rect(new Point(), sz));
						//bc.Child.UpdateLayout();
						fd.Blocks.Add(bc);
					}
				}

				pd.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "SeeShellsReport");
			}

			//Size pageSize = new Size(816, 1056);
		}
	}
}
