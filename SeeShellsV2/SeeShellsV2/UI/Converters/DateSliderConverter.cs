using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace SeeShellsV2.UI.Converters
{
	public class DateSliderConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DateTime result = new DateTime();
			if (parameter.ToString().Equals("StartDate"))
			{
				result = new DateTime(2020, 1, 1, 0, 0, 0).AddMinutes(double.Parse(value.ToString()));

				Debug.WriteLine("Startdateslider " + result);

			}
			if (parameter.ToString().Equals("EndDate"))
			{
				result = new DateTime(2020, 1, 1, 1, 0, 0).AddMinutes(double.Parse(value.ToString()));

				Debug.WriteLine("Enddateslider " + result);

			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
