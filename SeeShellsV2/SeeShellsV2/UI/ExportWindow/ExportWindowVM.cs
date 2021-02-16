using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public interface IExportWindowVM : IViewModel
    {
        IShellCollection ExportShellCollection { get; }
    }
    public class ExportWindowVM : ViewModel, IExportWindowVM
    {
        private IShellCollection collection = new ShellCollection();

        public IShellCollection ExportShellCollection
        {
            get => collection;
            set
            {
                collection = value;
                NotifyPropertyChanged();
            }
        }

    }
}
