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
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Win32;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace SeeShells.ShellParser.Registry
{
    public static class RegistryHelper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static DateTime? GetDateModified(RegistryHive registryHive, string path)
        {
            var lastModified = new FILETIME();
            var lpcbClass = new uint();
            var lpReserved = new IntPtr();
            UIntPtr key = UIntPtr.Zero;

            try
            {
                try
                {
                    var hive = new UIntPtr(unchecked((uint)registryHive));
                    if (RegOpenKeyEx(hive, path, 0, (int)RegistryRights.ReadKey, out key) != 0)
                    {
                        return null;
                    }

                    uint lpcbSubKeys;
                    uint lpcbMaxKeyLen;
                    uint lpcbMaxClassLen;
                    uint lpcValues;
                    uint maxValueName;
                    uint maxValueLen;
                    uint securityDescriptor;
                    StringBuilder sb;
                    if (RegQueryInfoKey(
                                 key,
                                 out sb,
                                 ref lpcbClass,
                                 lpReserved,
                                 out lpcbSubKeys,
                                 out lpcbMaxKeyLen,
                                 out lpcbMaxClassLen,
                                 out lpcValues,
                                 out maxValueName,
                                 out maxValueLen,
                                 out securityDescriptor,
                                 ref lastModified) != 0)
                    {
                        return null;
                    }

                    var result = ToDateTime(lastModified);
                    return result;
                }
                finally
                {
                    if (key != UIntPtr.Zero)
                        RegCloseKey(key);

                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"Couldn't retrieve registry modified date for {path}");
                return null;
            }
        }

        /// Helper Methods
        private static DateTime ToDateTime(FILETIME ft)
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                long[] longArray = new long[1];
                int cb = Marshal.SizeOf(ft);
                buf = Marshal.AllocHGlobal(cb);
                Marshal.StructureToPtr(ft, buf, false);
                Marshal.Copy(buf, longArray, 0, 1);
                return DateTime.FromFileTime(longArray[0]);
            }
            finally
            {
                if (buf != IntPtr.Zero) Marshal.FreeHGlobal(buf);
            }
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)] 
        private static extern int RegOpenKeyEx(
        UIntPtr hKey,
        string subKey,
        int ulOptions,
         int samDesired,
        out UIntPtr hkResult);

        [DllImport("advapi32.dll", EntryPoint = "RegQueryInfoKey", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern int RegQueryInfoKey(
        UIntPtr hkey,
        out StringBuilder lpClass,
        ref uint lpcbClass,
        IntPtr lpReserved,
        out uint lpcSubKeys,
        out uint lpcbMaxSubKeyLen,
        out uint lpcbMaxClassLen,
        out uint lpcValues,
        out uint lpcbMaxValueNameLen,
        out uint lpcbMaxValueLen,
        out uint lpcbSecurityDescriptor,
        ref FILETIME lpftLastWriteTime);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegCloseKey(UIntPtr hKey);
    }
}
