using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public interface IShellTag : IComparable<IShellTag>
    {
        /// <summary>
        /// Tag name, used as a unique identifier for the tag
        /// </summary>
        string Name { get; }

        /// <summary>
        /// relative importance score of the tag, normalized over [0,1)
        /// </summary>
        double Importance { get; }
        
        /// <summary>
        /// unique color associated with the tag
        /// </summary>
        Color Color { get; }
    }
}
