using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;
using System.Collections;
using SeeShellsV2.Modules;

namespace SeeShellsV2.UI
{
	public class ExportWindowVM : ViewModel, IExportWindowVM
	{
		[Dependency]
		public PdfExporter Exporter { get; set; }

		public IList moduleList { get; set; }

		public ExportWindowVM([Dependency] PdfExporter Export) 
		{
			moduleList = new List<IPdfModule>();
			moduleList.Add(Export.moduleNames["RTFModule"]);
		}

		public void Export_PDF()
		{
			Exporter.Export();
		}
	}
}
