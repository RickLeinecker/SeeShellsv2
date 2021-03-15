using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using SeeShellsV2.Data;

namespace SeeShellsV2.UI.Converters
{
    class ShellEventCollectionSorter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<IShellEvent> events)
                return events;// CollecionViewSource.From events.OrderBy(e => e.TimeStamp);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
