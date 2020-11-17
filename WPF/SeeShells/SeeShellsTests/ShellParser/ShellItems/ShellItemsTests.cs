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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.ShellParser.ShellItems;
using SeeShells.ShellParser.ShellItems.ExtensionBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser.Registry;
using SeeShellsTests.UI.Mocks;

namespace SeeShellsTests.ShellParser.ShellItems
{
    [TestClass]
    public class ShellItemsTests
    {
        private byte[] StringToByteArray(string str)
        {
            string[] items = str.Split('-');
            byte[] retval = new byte[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                retval[i] = Convert.ToByte(items[i], 16);
            }
            return retval;
        }

        private void printProperties(IShellItem shellItem)
        {
            string s = string.Join(";\n", shellItem.GetAllProperties().Select(x => x.Key + "=" + x.Value).ToArray());
            Console.WriteLine(s);

        }

        [TestMethod()]
        public void ShellItem0x1FTest()
        {
            //Represents the "My Computer" Root folder GUID - taken from Win7
            byte[] value = StringToByteArray("14-00-1F-50-E0-4F-D0-20-EA-3A-69-10-A2-D8-08-00-2B-30-30-9D-00-00");
            ShellItem0x1F shell = new ShellItem0x1F(value);
            printProperties(shell);

            Assert.AreEqual("20d04fe0-3aea-1069-a2d8-08002b30309d", shell.guid);
            Assert.AreEqual( "MY_COMPUTER", shell.folder_id);
        }

        [TestMethod()]
        public void ShellItem0x2FTest()
        {
            //Represents the F: Drive volume shell item - taken from Win7
            byte[] value = StringToByteArray(
                "19-00-2F-46-3A-5C-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00");
            ShellItem0x2F shell = new ShellItem0x2F(value);
            printProperties(shell);
            Assert.AreEqual("F:\\", shell.Name);
        }

        [TestMethod()]
        public void ShellItem0x30Test()
        {
            //Represents a file named "ATK_Package_win7_32_Z100008.zip" - taken from Win7
            //contains an extensions offset
            byte[] value = StringToByteArray(
                "88-00-32-00-C3-52-7D-00-85-47-1A-A7-20-00-41-54-4B-5F-50-41-7E-31-2E-5A-49" +
                "-50-00-00-6C-00-08-00-04-00-EF-BE-98-47-94-B9-98-47-94-B9-2A-00-00-00-64-E9" +
                "-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-41-00-54-00-4B" +
                "-00-5F-00-50-00-61-00-63-00-6B-00-61-00-67-00-65-00-5F-00-77-00-69-00-6E-00" +
                "-37-00-5F-00-33-00-32-00-5F-00-5A-00-31-00-30-00-30-00-30-00-30-00-38-00-2E" +
                "-00-7A-00-69-00-70-00-00-00-1C-00-00-00");

            ShellItem0x30 shell = new ShellItem0x30(value);
            printProperties(shell);
            Assert.AreEqual("8213187", shell.FileSize.ToString());
            Assert.AreEqual("32", shell.FileAttributes.ToString());
            Assert.AreEqual("0", shell.Flags.ToString());
            Assert.AreEqual("28", shell.ExtensionOffset.ToString());
            Assert.AreEqual("ATK_PA~1.ZIP", shell.ShortName);
        }

        [TestMethod()]
        public void ShellItem0x32ExtensionBEEF0004Test()
        {
            //Represents a file named "ATK_Package_win7_32_Z100008.zip" - taken from Win7
            //includes a extension block beef0004
            byte[] value = StringToByteArray(
                "88-00-32-00-C3-52-7D-00-85-47-1A-A7-20-00-41-54-4B-5F-50-41-7E-31-2E-5A-49" +
                "-50-00-00-6C-00-08-00-04-00-EF-BE-98-47-94-B9-98-47-94-B9-2A-00-00-00-64-E9" +
                "-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-41-00-54-00-4B" +
                "-00-5F-00-50-00-61-00-63-00-6B-00-61-00-67-00-65-00-5F-00-77-00-69-00-6E-00" +
                "-37-00-5F-00-33-00-32-00-5F-00-5A-00-31-00-30-00-30-00-30-00-30-00-38-00-2E" +
                "-00-7A-00-69-00-70-00-00-00-1C-00-00-00");

            ShellItem0x32 shell = new ShellItem0x32(value);
            printProperties(shell);

            //if full name is given, ShellItem successfully looked at the Extension Block
            Assert.AreEqual("ATK_Package_win7_32_Z100008.zip", shell.Name);
            Assert.AreEqual("12/24/2015 11:12:40 PM", shell.AccessedDate.ToString());
            Assert.AreEqual("12/24/2015 11:12:40 PM", shell.CreationDate.ToString());
            Assert.AreEqual(1, shell.ExtensionBlocks.Count);
            //TODO - find a way to test ALL extension block properties.
        }

        [TestMethod()]
        public void ShellItem0xC3Test()
        {
            //Network location with known location: \\192.168.80.129\Users
            byte[] value = StringToByteArray(
                "31-00-C3-01-C1-5C-5C-31-39-32-2E-31-36-38-2E-38-30-2E-31-32-39-5C-55-73-65-72" +
                "-73-00-4D-69-63-72-6F-73-6F-66-74-20-4E-65-74-77-6F-72-6B-00-00-02-00-00-00");

            ShellItem0xC3 shell = new ShellItem0xC3(value);
            printProperties(shell);

            Assert.AreEqual("193", shell.Flags.ToString());
            Assert.AreEqual("\\\\192.168.80.129\\Users", shell.Location.ToString());
            Assert.AreEqual("Microsoft Network", shell.Description.ToString());
            Assert.AreEqual(String.Empty, shell.Comments.ToString());


        }

        [TestMethod()]
        public void ShellItem0x61FTPHostAndUserTest()
        {
            //FTP hostname set to sonicfan1.tk with a username of a8849466, and conenct time 2017-01-17 23:53:14 
            //From Win7
            //this sample is special in that usernames dont usually appear. 
            byte[] value = StringToByteArray(
                "62-00-61-03-58-00-03-27-00-00-04-00-00-00-75-A1-4A-DE-1C-71-D2-01-00-00-00" +
                "-00-00-00-00-00-00-00-00-00-00-00-00-00-15-00-00-00-10-00-00-00-73-6F-6E" +
                "-69-63-66-61-6E-31-2E-74-6B-00-00-00-00-0C-00-00-00-61-38-38-34-39-34-36" +
                "-36-00-00-00-00-0C-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-66-74-70" +
                "-00-00-00");

            ShellItem0x61 shell = new ShellItem0x61(value);
            printProperties(shell);

            Assert.AreEqual("ftp", shell.Uri.ToString());
            Assert.AreEqual("sonicfan1.tk", shell.FTPHostname.ToString());
            Assert.AreEqual("a8849466", shell.FTPUsername.ToString());
            Assert.AreEqual(string.Empty, shell.FTPPassword.ToString());
            Assert.AreEqual("50354179", shell.Flags.ToString());
            Assert.AreEqual("1/17/2017 11:53:14 PM", shell.ConnectionDate.ToString());



        }
        [TestMethod()]
        public void ShellItem0x61FTPTest()
        {
            //FTP URI set to ftp and the FTPhostname set to 192.168.132.192 - From Win7
            //this sample is special in that usernames dont usually appear. 
            byte[] value = StringToByteArray(
                "52-00-61-03-48-00-03-01-00-00-04-00-00-00-DD-CA-82-1D-E9" +
                "-8F-D2-01-FF-FF-FF-FF-00-00-00-00-00-00-00-00-00-00-00" +
                "-00-15-00-00-00-10-00-00-00-31-39-32-2E-31-36-38-2E-31" +
                "-33-32-2E-31-39-32-00-04-00-00-00-00-00-00-00-04-00-00" +
                "-00-00-00-00-00-66-74-70-00-00-00");

            ShellItem0x61 shell = new ShellItem0x61(value);
            printProperties(shell);

            Assert.AreEqual("ftp", shell.Uri.ToString());
            Assert.AreEqual("192.168.132.192", shell.FTPHostname.ToString());
            Assert.AreEqual(string.Empty, shell.FTPUsername.ToString());
            Assert.AreEqual(string.Empty, shell.FTPPassword.ToString());
            Assert.AreEqual("50350083", shell.Flags.ToString());
            Assert.AreEqual("2/26/2017 4:30:53 AM", shell.ConnectionDate.ToString());


        }

        [TestMethod()]
        public void ShellItem0x71FTPTest()
        {
            //Control Panel item with 2227a280-3aea-1069-a2de-08002b30309d ("Printers") as the known GUID - From Win7
            byte[] value = StringToByteArray(
                "1E-00-71-80-00-00-00-00-00-00-00-00-00-00-80-A2-27-22-EA-3A" +
                "-69-10-A2-DE-08-00-2B-30-30-9D-00-00");

            ShellItem0x71 shell = new ShellItem0x71(value);
            printProperties(shell);

            Assert.AreEqual("128", shell.Flags.ToString());
            Assert.AreEqual("2227a280-3aea-1069-a2de-08002b30309d", shell.Guid.ToString());
        }



        /// <summary>
        /// Checks that invalid Modified datestamp times (e.g. 4 0x00 bytes) is correctly parsed.
        /// If it isnt, the DateTime specification with thrown an exception. (DateTime cant take 00 for year,month, or day)
        /// </summary>
        [TestMethod()]
        public void ShellItem0x31InvalidTimeTest()
        {
            //File Shell Item - Directory with Beef0004 extension with "AppData" as the Name.
            byte[] value = { 0x56, 0x00, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10,
                0x00, 0x41, 0x70, 0x70, 0x44, 0x61, 0x74, 0x61, 0x00, 0x40, 0x00, 0x09, 0x00, 0x04, 0x00, 0xef,
                0xbe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41, 0x00, 0x70, 0x00, 0x70, 0x00, 0x44, 0x00, 0x61,
                0x00, 0x74, 0x00, 0x61, 0x00, 0x00, 0x00, 0x16, 0x00, 0x00, 0x00 };

            ShellItem0x31 shell = new ShellItem0x31(value);
            printProperties(shell);

            //Test passes by throwing no Exceptions.
        }

        [TestMethod()]
        public void ShellItem0x74InvalidDateTest()
        {
            byte[] value =
                {
                    0x82, 0x00, 0x74, 0x00, 0x1e, 0x00, 0x43, 0x46, 0x53, 0x46, 0x18, 0x00, 0x31, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x99, 0x47, 0xcf, 0x05, 0x11, 0x00, 0x4f, 0x6e, 0x65, 0x44, 0x72, 0x69,
                    0x76, 0x65, 0x00, 0x00, 0x00, 0x00, 0x74, 0x1a, 0x59, 0x5e, 0x96, 0xdf, 0xd3, 0x48, 0x8d,
                    0x67, 0x17, 0x33, 0xbc, 0xee, 0x28, 0xba, 0xc5, 0xcd, 0xfa, 0xdf, 0x9f, 0x67, 0x56, 0x41,
                    0x89, 0x47, 0xc5, 0xc7, 0x6b, 0xc0, 0xb6, 0x7f, 0x3e, 0x00, 0x08, 0x00, 0x04, 0x00, 0xef,
                    0xbe, 0x99, 0x47, 0xcf, 0x05, 0x99, 0x47, 0xcf, 0x05, 0x2a, 0x00, 0x00, 0x00, 0x8a, 0xfe,
                    0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x4f, 0x00, 0x6e, 0x00, 0x65, 0x00, 0x44, 0x00, 0x72, 0x00,
                    0x69, 0x00, 0x76, 0x00, 0x65, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00
                };
            ShellItem0x74 shell = new ShellItem0x74(value);
            printProperties(shell);

            //test passes if no exceptions are thrown.
        }


        /// <summary>
        /// Checks if values from a <see cref="RegistryKeyWrapper"/> can inject additional properties to a shellitem.
        /// </summary>
        [TestMethod()]
        public void ShellItemRegistryDecoratorTest()
        {
            var shellItem = new MockShellItem("Test", 0x99);
            var regKey = new RegistryKeyWrapper(null, "testUser", "testPath");

            int existingPropCount = shellItem.GetAllProperties().Count;

            //test injection of additional properties
            var regShellItem = new RegistryShellItemDecorator(shellItem, regKey);

            Assert.IsTrue(regShellItem.GetAllProperties().Count > existingPropCount);
        }
    }
}