using SeeShellsV2.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Services
{
	public interface IPdfExporter
	{
		public void Export(string filename, ObservableCollection<IPdfModule> moduleList);
	}
}
