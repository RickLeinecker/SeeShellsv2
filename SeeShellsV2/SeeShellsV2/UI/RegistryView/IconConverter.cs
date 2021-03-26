using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using SeeShellsV2.Data;

namespace SeeShellsV2.UI
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RegistryHive)
                return "Cubes";

            if (value is VolumeShellItem)
                return "Harddrive";

            if (value is MtpDeviceShellItem)
                return "Mobile";

            return "FolderOpen";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
