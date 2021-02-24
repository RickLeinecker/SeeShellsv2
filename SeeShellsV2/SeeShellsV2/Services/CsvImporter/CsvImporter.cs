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
        private readonly IShellItemCollection shellItems;
        private readonly IShellItemFactory shellFactory;

        public CsvImporter(
            [Dependency] IShellItemCollection shellItems,
            [Dependency] IShellItemFactory shellFactory
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
                UseNewObjectForNullReferenceMembers = false
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
                    var typeId = csv.GetField<string>("Type");
                    Type type = shellFactory.GetShellType(byte.Parse(typeId, NumberStyles.HexNumber));

                    if (type == null)
                    {
                        csv.Read();
                        continue;
                    }

                    shellItems.Add((IShellItem)csv.GetRecord(type));
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
