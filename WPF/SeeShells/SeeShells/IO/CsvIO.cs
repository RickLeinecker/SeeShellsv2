#region copyright
// SeeShells Copyright (c) 2019-2020 Aleksandar Stoyanov, Bridget Woodye, Klayton Killough, 
// Richard Leinecker, Sara Frackiewicz, Yara As-Saidi
// SeeShells is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// SeeShells is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, see <https://www.gnu.org/licenses>
#endregion
using CsvHelper;
using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NLog;
using SeeShells.UI;

namespace SeeShells.IO
{
    /// <summary>
    /// This class is used to manage output of ShellBag data to a CSV file and import of ShellBag data from a CSV file to main memory.
    /// </summary>
    public static class CsvIO
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a CSV file with ShellBag data in a given location.
        /// </summary>
        /// <param name="shellItems">A list of ShellItems containg the data to be exported in the CSV file</param>
        /// <param name="filePath">The directory in which to output the CSV file including the CSV file name</param>
        /// <returns></returns>
        public static void OutputCSVFile(List<IShellItem> shellItems, String filePath)
        {
            // Reads each propery key of each ShellItem in order to determine the headers of the CSV.
            HashSet<String> keys = new HashSet<String>();
            foreach (IShellItem shellItem in shellItems)
            {
                foreach (KeyValuePair<string, string> property in shellItem.GetAllProperties())
                {
                    keys.Add(property.Key);
                } 
            }

            // Converts the HashSet of CSV headers to an array in order to alphabetize the headers and
            // establish indexes to be used to track the columns in which the property values should go.
            String[] keysArray = new String[keys.Count];
            keys.CopyTo(keysArray);
            Array.Sort(keysArray);

            // Converts the array of alphabetized headers to a Map with the array indexes as values.
            Dictionary<string, int> keysMap = new Dictionary<string, int>();
            for(int i = 0; i < keysArray.Length; i++)
            {
                keysMap.Add(keysArray[i], i);
            }

            // Writes the headers to the CSV and parses over the properties of each ShellItem again in order to obtain the values
            // for each property, which are then placed in an array acording to the index that they map to and written to the CSV.
            using (var writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(string.Join(",", keysArray));
                writer.Flush();

                foreach (IShellItem shellItem in shellItems)
                {
                    String[] line = new String[keysArray.Length];
                    foreach (KeyValuePair<string, string> property in shellItem.GetAllProperties())
                    {
                        line[keysMap[property.Key]] = "\"" + property.Value.Replace("\"", "\"\"") + "\""; // The whole value is enclosed in double quotes to retain any special characters.
                                                                                                          // Any preexisting double quotes are enclosed in double quotes to preserve them.
                    }
                    writer.WriteLine((string.Join(",", line)));
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Creates a ShellItem list from ShellBag data parsed from a CSV.
        /// </summary>
        /// <param name="filePath">The path to a source CSV file</param>
        /// <returns>A list of ShellItems</returns>
        public static List<IShellItem> ImportCSVFile(String filePath)
        {
            List<IShellItem> retList = new List<IShellItem>();
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    string[] propertyValues = csv.Context.HeaderRecord;
                    int propertyValuesCurrIndex = 0;
                    int failedReadRows = 0;

                    while (csv.Read())
                    {
                        try
                        {
                            IDictionary<string, string> properties = new Dictionary<string, string>();
                            for (int i = 0; i < csv.Context.Record.Length; i++)
                            {
                                if (!csv.GetField(i).Equals(""))
                                {
                                    properties.Add(propertyValues[propertyValuesCurrIndex], csv.GetField(i));
                                }

                                propertyValuesCurrIndex++;
                            }

                            propertyValuesCurrIndex = 0;
                            retList.Add(new CsvParsedShellItem(properties));
                        }
                        catch (Exception ex)
                        {
                            failedReadRows++;
                            string message;

                            if (ex is KeyNotFoundException)
                            {
                                message = $"Cannot extract events from Shellbag at row {csv.Context.RawRow}. Missing required information, See log.txt for more information.";
                            }
                            else
                            {
                                message = $"Failed to read row {csv.Context.RawRow} in the CSV file.";
                            }

                            logger.Warn(message);
                            LogAggregator.Instance.Add(message);
                        }
                    }

                    //too many failed rows, just report that multiple failed (if all failed, dont report)
                    if (failedReadRows > 10 && retList.Count != 0)
                    {
                        LogAggregator.Instance.Clear();
                        LogAggregator.Instance.Add($"Failed to read multiple in the CSV file. See log.txt for more information.");
                    } else if (retList.Count == 0)
                    {
                        LogAggregator.Instance.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Failed to read CSV file {filePath}";
                logger.Error(ex, message);
                LogAggregator.Instance.Add(message);
                return retList;
            }

            if (retList.Count == 0)
            {
                string message = $"No Shellbag records found in file.";
                logger.Info(message);
                LogAggregator.Instance.Add(message);
            }

            return retList;
        }
    }
}
