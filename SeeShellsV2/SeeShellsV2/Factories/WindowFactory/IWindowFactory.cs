using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.UI;

namespace SeeShellsV2.Factories
{
    public interface IWindowFactory
    {
        /// <summary>
        /// Create a new window
        /// </summary>
        /// <param name="subtype">name of specific window to create</param>
        /// <returns>a new window</returns>
        IWindow Create(string subtype = null);
    }
}
