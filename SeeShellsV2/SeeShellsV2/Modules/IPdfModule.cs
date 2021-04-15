using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeShellsV2.Modules
{
	public interface IPdfModule
	{
		public string Name { get; }
		public void Render(Section section);
		public FrameworkElement View();
		public IPdfModule Clone();
	}
}
