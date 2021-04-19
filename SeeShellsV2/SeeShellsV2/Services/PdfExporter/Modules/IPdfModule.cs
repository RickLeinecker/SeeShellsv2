using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeShellsV2.Services
{
	public interface IPdfModule
	{
		public string Name { get; }
		public UIElement Render();
		public FrameworkElement View();
		public IPdfModule Clone();
	}
}
