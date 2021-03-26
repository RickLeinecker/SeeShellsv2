using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public class RegistryCollection
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public INotifyCollectionChanged Items { get; set; }
    }

    public class RegistryCollectionsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //get collection name listing...
            string p = parameter as string ?? "";
            var names = p.Split(',').Select(f => f.Trim()).ToList();
            //...and make sure there are no missing entries
            while (2.0 * values.Length > names.Count) names.Add(String.Empty);

            List<RegistryCollection> items = new List<RegistryCollection>();

            int idx = 0;
            foreach (var value in values)
            {
                if (value is INotifyCollectionChanged n)
                    items.Add(new RegistryCollection { Name = names[idx], Icon = names[idx + values.Length], Items = n });

                idx++;
            }

            return items;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
