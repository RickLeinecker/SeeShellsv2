using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeShellsV2.UI
{
    public class UtcToLocalTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime d)
            {
                string zone = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                    TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.DisplayName;

                zone = Regex.Replace(zone, "[^A-Z]", "");

                string s = parameter as string;
                switch (s)
                {
                    case "ShortDate":
                        return d.ToLocalTime().ToShortDateString() + ' ' + zone;
                    case "ShortTime":
                        return d.ToLocalTime().ToShortTimeString() + ' ' + zone;
                    case "LongDate":
                        return d.ToLocalTime().ToLongDateString() + ' ' + zone;
                    case "LongTime":
                        return d.ToLocalTime().ToLongTimeString() + ' ' + zone;
                    default:
                        return d.ToLocalTime().ToString() + ' ' + zone;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
