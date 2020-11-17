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
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.IO.Networking
{
    [TestClass()]
    public class JSONTests
    {

        [TestMethod]
        public void APIError_DeserializationTest()
        {
            var json = "{\"success\": 0, \"error\": \"Error Occurred\"}";
            var obj = JsonConvert.DeserializeObject<APIError>(json);
            Assert.AreEqual(obj.GetType(), new APIError().GetType());
        }

        [TestMethod]
        public void GUIDPair_DeserializationTest()
        {
            string json = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleGUIDs.json");
            var obj = JsonConvert.DeserializeObject<IList<GUIDPair>>(json);
            Assert.AreEqual(obj[0].GetType(), new GUIDPair().GetType());
        }


        [TestMethod]
        public void RegistryLocations_DeserializationTest()
        {
            string json = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleOSRegistryLocations.json");
            var obj = JsonConvert.DeserializeObject<IList<RegistryLocations>>(json);
            Assert.AreEqual(obj[0].GetType(), new RegistryLocations().GetType());
        }

    }
}
