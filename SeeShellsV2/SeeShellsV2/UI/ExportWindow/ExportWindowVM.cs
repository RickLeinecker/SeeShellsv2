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
using System.Threading;

namespace SeeShellsV2.UI
{
	public class ExportWindowVM : ViewModel, IExportWindowVM
	{
		[Dependency]
		public PdfExporter Exporter { get; set; }

		public IList moduleList { get; set; }

		public string Status
		{
			get => _status;
			set { _status = value; NotifyPropertyChanged(); }
		}

		private string _status = string.Empty;

		public ExportWindowVM([Dependency] PdfExporter Export) 
		{
			moduleList = new List<IPdfModule>();
			moduleList.Add(Export.moduleNames["RTFModule"]);

			Status = "Save";
		}

		public async void Export_PDF(string filename)
		{
			Status = "Saving...";
			await Task.Run(() => Exporter.Export(filename));
			Status = "Saved";
			await Task.Run(() => Thread.Sleep(5000));
			Status = "Save";
		}
	}
}
