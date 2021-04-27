using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Services
{
	/// <summary>
	/// A service that handles PDF report generation
	/// </summary>
	public interface IPdfExporter
	{
		/// <summary>
		/// Renders the given <see cref="IPdfModule"/> instances to a paginated report and then
		/// sends the report to a Windows Print Dialog
		/// </summary>
		/// <param name="moduleList">A list of modules to print</param>
		public void Export(IEnumerable<IPdfModule> moduleList);
	}
}
