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
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using SeeShells.ShellParser.Registry;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.ShellParser
{
    public class Program
    {
        public static void Leinecker()
        {
            if (File.Exists("output.csv"))
                File.Delete("output.csv");
            using (var writer = new StreamWriter("output.csv", true))
            {
                var line = "File Path,Slot,Slot Modified Date/Time,Slot Key,Key,RegKey Modified Date/Time,Modified Date/Time,Accessed Date/Time,Created Date/Time";

                writer.WriteLine(line);
                writer.Flush();
            }
            iterateCurrentShellbags();
        }

        static void iterateCurrentShellbags()
        {
            RegistryKey rk = Microsoft.Win32.Registry.CurrentUser;
            string sk = "Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell";
            iterateRegistry(rk.OpenSubKey(sk), sk, 0, "");
        }

        static void iterateRegistry(RegistryKey rk, string subKey, int indent, string path_prefix)
        {
            if (rk == null)
            {
                return;
            }

            string[] subKeys = rk.GetSubKeyNames();
            string[] values = rk.GetValueNames();

            Console.WriteLine("**" + subKey);

            foreach (string valueName in subKeys)
            {
                if (valueName.ToUpper() == "ASSOCIATIONS")
                {
                    continue;
                }

                string sk = getSubkeyString(subKey, valueName);
                Console.WriteLine("{0}", sk);
                RegistryKey rkNext = rk.OpenSubKey(valueName);
                int slot = 0;
                DateTime slotModified = DateTime.MinValue;
                string slotKeyName = "";
                try
                {
                    slot = (int)rk.GetValue("NodeSlot");
                    slotKeyName = string.Format("{0}{1}\\{2}", rk.Name.Substring(0, rk.Name.IndexOf("BagMRU")), "Bags", slot);
                    if (rk.Name.StartsWith("HKEY_USERS"))
                    {
                        slotModified = RegistryHelper.GetDateModified(RegistryHive.Users, slotKeyName.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;
                    }
                    else if (rkNext.Name.StartsWith("HKEY_CURRENT_USER"))
                    {
                        slotModified = RegistryHelper.GetDateModified(RegistryHive.CurrentUser, slotKeyName.Replace("HKEY_CURRENT_USER\\", "")) ?? DateTime.MinValue;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("NodeSlot was not found");
                }

                int intVal = 0;
                string path = path_prefix;
                bool isNumeric = int.TryParse(valueName, out intVal);
                if (isNumeric)
                {
                    try
                    {
                        byte[] byteVal = (byte[])rk.GetValue(valueName);
                        SHITEMLIST l = new SHITEMLIST(byteVal, 0, null);

                        foreach (ShellItem item in l.items())
                        {
                            if (path_prefix.Length == 0)
                                path = item.Name;
                            else
                                path = path_prefix + "\\" + item.Name;
                            Dictionary<string, object> shellbag = new Dictionary<string, object>() {
                                { "path", path},
                                { "mtime", item.ModifiedDate },
                                { "atime", item.AccessedDate },
                                { "crtime", item.CreationDate },
                                { "source", string.Format("{0} @ {1}", subKey, item.offset) },
                                { "regsource", rkNext.Name }
                            };
                            DateTime dateModified = DateTime.MinValue;
                            if (rkNext.Name.StartsWith("HKEY_USERS"))
                            {
                                dateModified = RegistryHelper.GetDateModified(RegistryHive.Users, rkNext.Name.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;
                            }
                            else if (rkNext.Name.StartsWith("HKEY_CURRENT_USER"))
                            {
                                dateModified = RegistryHelper.GetDateModified(RegistryHive.CurrentUser, rkNext.Name.Replace("HKEY_CURRENT_USER\\", "")) ?? DateTime.MinValue;
                            }
                            using (var writer = new StreamWriter("output.csv", true))
                            {
                                var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                                    shellbag["path"],
                                    slot,
                                    slotModified,
                                    slotKeyName,
                                    shellbag["regsource"],
                                    dateModified,
                                    shellbag["mtime"],
                                    shellbag["atime"],
                                    shellbag["crtime"]);
                                writer.WriteLine(line);
                                writer.Flush();
                            }
                        }
                    }
                    catch (OverrunBufferException ex)
                    {
                        Console.WriteLine("OverrunBufferException: " + valueName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(valueName);
                    }
                }

                iterateRegistry(rkNext, sk, indent + 2, path);
            }

        }

        ///Helper Methods
        static string getSubkeyString(string subKey, string addOn)
        {
            return String.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
