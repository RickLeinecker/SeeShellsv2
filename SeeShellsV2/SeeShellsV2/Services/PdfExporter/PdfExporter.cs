using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SeeShellsV2.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

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

			ExportPdf(filename, CreateReport(moduleList));
			//PdfDocument doc = new PdfDocument();
			//// Organize Views
			//PdfPage[] pages = new PdfPage[(int)Math.Ceiling((double)moduleList.Count / 5)];
			//for (int j = 0; j < pages.Length; j++)
			//{
			//	pages[j] = doc.AddPage();

			//}
			//int i = 1;
			//foreach (IPdfModule module in moduleList)
			//{
			//	module.Render(pages[((int)Math.Ceiling((double)i / 5)) - 1]);
			//	i++;
			//	Debug.WriteLine("Page " + i);
			//}
			//Debug.WriteLine(pages.Length);
			//int numPages = doc.PageCount;
			//doc.Save(filename);
		}

		private void ExportPdf(string path, Document report)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var pdfRenderer = new PdfDocumentRenderer();
			pdfRenderer.Document = report;
			pdfRenderer.RenderDocument();
			pdfRenderer.PdfDocument.Save(path);
		}

		private Document CreateReport(ObservableCollection<IPdfModule> moduleList)
		{
			var doc = new Document();
			var sec = new Section();
			foreach (IPdfModule module in moduleList)
			{
				module.Render(sec);
			}
			doc.Add(sec);
			return doc;
		}
	}
}
