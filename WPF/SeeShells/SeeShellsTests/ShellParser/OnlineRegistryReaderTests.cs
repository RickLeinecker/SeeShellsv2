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
using SeeShells.ShellParser;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System.Collections.Generic;
using SeeShells.ShellParser.Registry;

namespace SeeShellsTests.ShellParser
{
    [TestClass()]
    public class OnlineRegistryReaderTests
    {
        /// <summary>
        /// Tests that the Online Registry Reader can read from a running system's registry.
        /// </summary>
        [TestMethod()]
        [TestCategory("OnlineTest")]
        public void GetRegistryKeysTest()
        {
            OnlineRegistryReader registryReader = new OnlineRegistryReader(new MockConfigParser());
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();

            Assert.AreNotEqual(keys.Count, 0);
        }

    }
}
