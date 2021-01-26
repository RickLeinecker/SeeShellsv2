using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Unity;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using SeeShellsV2.Data;
using SeeShellsV2.Factories;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.Services
{
    public class CsvImporter : ICsvImporter
    {
        private readonly IShellCollection shellItems;
        private readonly IAbstractFactory<IShellItem> shellFactory;

        public CsvImporter(
            [Dependency] IShellCollection shellItems,
            [Dependency] IAbstractFactory<IShellItem> shellFactory
        )
        {
            this.shellItems = shellItems;
            this.shellFactory = shellFactory;
        }

        public void Import(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
            };
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.TypeConverterCache.AddConverter<byte>(new HexByteFieldConverter());
                csv.Context.TypeConverterCache.AddConverter<ushort>(new HexUshortFieldConverter());

                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var type = csv.GetField<string>("Type");
                    IShellItem item = shellFactory.Create(type);
                    shellItems.Add(csv.GetRecord(item.GetType()) as IShellItem);
                }
            }
        }
    }

    internal class HexByteFieldConverter : ITypeConverter
    {
        public object ConvertFromString(string s, IReaderRow r, MemberMapData m)
        {
            return byte.Parse(s, NumberStyles.HexNumber);
        }

        public string ConvertToString(object o, IWriterRow r, MemberMapData m)
        {
            return string.Format("{0:X}", o);
        }
    }

    internal class HexUshortFieldConverter : ITypeConverter
    {
        public object ConvertFromString(string s, IReaderRow r, MemberMapData m)
        {
            return ushort.Parse(s, NumberStyles.HexNumber);
        }

        public string ConvertToString(object o, IWriterRow r, MemberMapData m)
        {
            return string.Format("{0:X}", o);
        }
    }
}
