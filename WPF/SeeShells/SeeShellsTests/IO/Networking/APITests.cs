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
using MockHttpServer;
using RestSharp;
using SeeShells.IO.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.IO.Networking
{
    /// <summary>
    /// Testing class for various API method calls. 
    /// All test should use <see cref="MockServer"/> to emulate the server calls without the need of external dependencies. 
    /// </summary>
    [TestClass()]
    public class APITests
    {
        static readonly int TEST_PORT = 4935; //dont use port 80 because the testing system might already have a webserver
        static readonly string TEST_URL = $"http://localhost:{TEST_PORT}";

        /// <summary>
        /// Test the ability to resolve an API location with <see cref="API.GetAPIClient(string, string)"/>
        /// </summary>
        [TestMethod()]
        public void GetAPIClientTest()
        {

            string testApiHost = "http://localhost/api";

            using (new MockServer(TEST_PORT, API.DEFAULT_WEBSITE_API_ENDPOINT, (req, rsp, prm) => testApiHost))
            {
                var apiClient = API.GetAPIClient(TEST_URL);
                Assert.IsNotNull(apiClient);
                Assert.AreEqual(testApiHost, apiClient.Result.BaseUrl.ToString());
            }
        }

        /// <summary>
        /// Tests the exception throwing when <see cref="API.GetAPIClient(string, string)"/> fails 
        /// </summary>
        [TestMethod()]
        public void GetAPIClient_ServerErrorTest()
        {
               
            try
            {
                var apiClient = API.GetAPIClient("http://localhost:" + (TEST_PORT+1)).Result;
                Assert.Fail("Expected Exception"); //expect exception

            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType());
            }

        }

        /// <summary>
        /// Checks the functionality of the <see cref="API.GetGuids(string, IRestClient)"/> function
        /// </summary>
        [TestMethod()]
        public void GetGuidsTest()
        {

            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleGUIDsResponse.json");
            string serializedJson = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleGUIDs.json");


            using (new MockServer(TEST_PORT, API.GUID_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                var testOutputPath = API.GetGuids("output.json", apiClient).Result;

                Assert.IsNotNull(apiClient);
                Assert.AreEqual(serializedJson, File.ReadAllText(testOutputPath));
                File.Delete(testOutputPath);
            }
        }
        /// <summary>
        /// Tests the exception throwing when <see cref="API.GetGuids(string, IRestClient)"/> fails 
        /// </summary>
        [TestMethod()]
        public void GetGuids_ServerErrorTest()
        {
            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleAPIError.json");

            using (new MockServer(TEST_PORT, API.GUID_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                try
                {
                    var testOutputPath = API.GetGuids("output.json", apiClient).Result;
                    Assert.Fail("Expected Exception"); //expect exception


                }
                catch (AggregateException ex)
                {
                    Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType()); 
                }

            }

        }

        /// <summary>
        /// Checks the functionality of the <see cref="API.GetOSRegistryLocations(string, IRestClient)"/> function
        /// </summary>
        [TestMethod()]
        public void GetOSRegistryLocationsTest()
        {

            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleOSRegistryLocationsResponse.json");
            string serializedJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleOSRegistryLocations.json");

            using (new MockServer(TEST_PORT, API.OS_REGISTRY_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                var testOutputPath = API.GetOSRegistryLocations("output.json", apiClient).Result;

                Assert.IsNotNull(apiClient);
                Assert.AreEqual(serializedJSON, File.ReadAllText(testOutputPath));
                File.Delete(testOutputPath);
            }
        }

        /// <summary>
        /// Tests the exception throwing when <see cref="API.GetOSRegistryLocations(string, IRestClient)"/> fails.
        /// </summary>
        [TestMethod()]
        public void GetOSRegistryLocations_ServerErrorTest()
        {
            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleAPIError.json");

            using (new MockServer(TEST_PORT, API.OS_REGISTRY_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                try
                {
                    var testOutputPath = API.GetOSRegistryLocations("output.json", apiClient).Result;
                    Assert.Fail("Expected Exception"); //expect exception
                }
                catch (AggregateException ex)
                {
                    Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType());
                }

            }

        }

        /// <summary>
        /// Checks the functionality of the <see cref="API.GetHelp(string, IRestClient)"/> function
        /// </summary>
        [TestMethod()]
        public void GetHelpTest()
        {
            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleHelpResponse.json");
            string serializedText = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleHelp.md");

            using (new MockServer(TEST_PORT, API.HELP_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                var helpContent = API.GetHelp(apiClient).Result;

                Assert.IsNotNull(apiClient);
                Assert.AreEqual(serializedText, helpContent);
            }

        }
        /// <summary>
        /// Tests the exception throwing when <see cref="API.GetHelp(string, IRestClient)"/> fails.
        /// </summary>
        [TestMethod()]
        public void GetHelp_ServerErrorTest()
        {
            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleAPIError.json");

            using (new MockServer(TEST_PORT, API.HELP_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                try
                {
                    var helpResult = API.GetHelp(apiClient).Result;
                    Assert.Fail("Expected Exception"); //expect exception
                }
                catch (AggregateException ex)
                {
                    Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType());
                }
            }

        }

    }
}