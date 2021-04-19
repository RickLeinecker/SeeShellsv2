using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using SeeShellsV2.Data;

namespace SeeShellsV2.UI
{
    public class StreamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] buf)
            {
                MemoryStream m = new MemoryStream();
                m.Write(buf, 0, buf.Length);
                return m;
            }

            if (value is IShellItem i)
            {
                MemoryStream m = new MemoryStream();
                m.Write(i.Value, 0, i.Value.Length);
                return m;
            }

            if (value is IShellEvent e)
            {
                MemoryStream m = new MemoryStream();
                m.Write(e.Evidence.First().Value, 0, e.Evidence.First().Value.Length);
                return m;
            }

            if (value is Place p)
            {
                MemoryStream m = new MemoryStream();
                m.Write(p.Items.First().Value, 0, p.Items.First().Value.Length);
                return m;
            }


            return new MemoryStream(Array.Empty<byte>());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
