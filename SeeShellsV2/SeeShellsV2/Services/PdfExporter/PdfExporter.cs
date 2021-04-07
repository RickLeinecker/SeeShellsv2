using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SeeShellsV2.Modules;
using System;
using System.Collections.Generic;
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

		public void Export(string filename)
		{
			PdfDocument doc = new PdfDocument();
			// Organize Views
			int pages = doc.PageCount;
			doc.Save(filename);
		}
	}
}
