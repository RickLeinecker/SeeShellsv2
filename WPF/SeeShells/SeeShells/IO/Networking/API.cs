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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SeeShells.IO.Networking.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SeeShells.IO.Networking
{
    public class API
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private static IRestClient apiClient = null;

        public const string DEFAULT_WEBSITE_URL = "https://rickleinecker.github.io/SeeShells";
        public const string DEFAULT_WEBSITE_API_ENDPOINT = "apiEndpoint.txt";

        public const string GUID_ENDPOINT = "getGUIDs";
        private const string DEFAULT_GUID_FILENAME = "SeeShellsGUIDS.json";

        public const string OS_REGISTRY_ENDPOINT = "getOSandRegistryLocations";
        private const string DEFAULT_OS_REGISTRY_FILENAME = "SeeShellsRegistryLocations.json";

        public const string SCRIPTS_ENDPOINT = "getScripts";
        private const string DEFAULT_SCRIPTS_FILENAME = "SeeShellsScripts.json";

        public const string HELP_ENDPOINT = "getHelpInformation";

        /// <summary>
        /// Obtains a <see cref="RestClient"/> which is ready to send requests to the SeeShells API Server.
        /// To do so, it must make a request to a special endpoint on the SeeShells website to obtain the API's location. (see SeeShells Design Doc)
        /// </summary>
        /// <param name="url">URL of the SeeShells Website that contains the special endpoint.</param>
        /// <param name="endpoint">An endpoint which returns the URL of the API.</param>
        /// <returns>A Task containing a Rest Client ready to make requests to the API.</returns>
        /// <exception cref="IOException"> When any networking error occurs during the operation.</exception>
        public static async Task<IRestClient> GetAPIClient(string url = DEFAULT_WEBSITE_URL, string endpoint = DEFAULT_WEBSITE_API_ENDPOINT)
        {
            if (url != DEFAULT_WEBSITE_URL)
                apiClient = null; //meant for unit testing ONLY. If we allow a configurable website later on THIS HAS TO BE REMOVED.

            //"cache" the location of the API, so dont do the network call again if we dont have to.
            if (apiClient == null) { 
                RestClient client = new RestClient(url);

                RestRequest request = new RestRequest(endpoint, DataFormat.None);
                //hit the website to get the URL of the API endpoint
                IRestResponse response = await client.ExecuteGetAsync(request);
                if (response.ErrorException == null)
                { 
                    //convert API url to Uri
                    Uri apiURL = new Uri(response.Content);
                    apiClient = new RestClient(apiURL);
                } 
                else //without the API endpoint we cant proceed 
                {
                    throw new APIException("Couldnt obtain API website", response.ErrorException);
                }
            }
            return apiClient;
        }

        /// <summary>
        /// Obtains a list of Shellbag GUIDS from the SeeShells API in JSON format and writes to a file
        /// </summary>
        /// <param name="outputFilePath">Specific file path in which to save the results. If it doesnt exist it will be created.</param>
        /// <param name="apiClient">A custom client to use for the GET operation.</param>
        /// <returns>the filepath in which the data was saved.</returns>
        /// <exception cref="IOException"> When any networking or file error occurs during the operation.</exception>
        public static async Task<string> GetGuids(string outputFilePath, IRestClient apiClient = null)
        {
            RestRequest guidRequest = new RestRequest(GUID_ENDPOINT, DataFormat.Json);
            IList<GUIDPair> data = await PerformGET<IList<GUIDPair>>(guidRequest, apiClient);
            return WriteToFile(data, outputFilePath, DEFAULT_GUID_FILENAME);
        }

        /// <summary>
        /// Obtains a list of Operating Systems and their respective Registry file key paths for Shellbag information from the SeeShells API in JSON format and writes to a file.
        /// </summary>
        /// <param name="outputFilePath">Specific file path in which to save the results. If it doesnt exist it will be created.</param>
        /// <param name="apiClient">A custom client to use for the GET operation.</param>
        /// <returns>the filepath in which the data was saved.</returns>
        /// <exception cref="IOException"> When any networking or file error occurs during the operation.</exception>
        public static async Task<string> GetOSRegistryLocations(string outputFilePath, IRestClient apiClient = null)
        {
            RestRequest guidRequest = new RestRequest(OS_REGISTRY_ENDPOINT, DataFormat.Json);
            IList<RegistryLocations> data = await PerformGET<IList<RegistryLocations>>(guidRequest, apiClient);
            return WriteToFile(data, outputFilePath, DEFAULT_OS_REGISTRY_FILENAME);
        }

        /// <summary>
        /// Obtains a list of Shellbag scripts from the SeeShells API in JSON format and writes to a file
        /// </summary>
        /// <param name="outputFilePath">Specific file path in which to save the results. If it doesnt exist it will be created.</param>
        /// <param name="apiClient">A custom client to use for the GET operation.</param>
        /// <returns>the filepath in which the data was saved.</returns>
        /// <exception cref="IOException"> When any networking or file error occurs during the operation.</exception>
        public static async Task<string> GetScripts(string outputFilePath, IRestClient apiClient = null)
        {
            RestRequest scriptsRequest = new RestRequest(SCRIPTS_ENDPOINT, DataFormat.Json);
            IList<ScriptPair> data = await PerformGET<IList<ScriptPair>>(scriptsRequest, apiClient);
            return WriteToFile(data, outputFilePath, DEFAULT_SCRIPTS_FILENAME);
        }
        /// <summary>
        /// Obtains the content for the help section of the WPF
        /// </summary>
        /// <param name="apiClient">A custom client to use for the GET operation.</param>
        /// <returns>The help text received</returns>
        public static async Task<string> GetHelp(IRestClient apiClient = null)
        {
            return await Task.Run(async () =>
            {
                RestRequest helpRequest = new RestRequest(HELP_ENDPOINT, DataFormat.Json);
                IList<HelpItem> data = await PerformGET<IList<HelpItem>>(helpRequest, apiClient);

                return data.First().GetHelpContent();
            });
        }


        /// <summary>
        /// Performs a GET request on an API Endpoint
        /// </summary>
        /// <param name="restRequest">The Request to perform</param>
        /// <param name="apiClient">The client to use for the GET operation.</param>
        /// <returns>An absolute filepath of the written file</returns>
        /// <exception cref="IOException"> When any networking or file error occurs during the operation.</exception>
        /// <typeparam name="T">A Object which can be serialized from json for validty checking.</typeparam>
        private static async Task<T> PerformGET<T>(RestRequest restRequest, IRestClient apiClient)
        {
            if (apiClient == null)
            {
                apiClient = await GetAPIClient();
            }

            IRestResponse response = apiClient.Get(restRequest);
            if (response.ErrorException == null)
            {
                CheckAPIError(response);
                return JsonConvert.DeserializeObject<T>(UnwrapJSONContainer(response));
            }

            throw new APIException("Networking Error: " + response.ErrorException.Message, response.ErrorException);
        }

        /// <summary>
        /// Writes a file to the filesystem. 
        /// Resorts to the current directory of the program if no output file is specified. 
        /// </summary>
        /// <param name="content">The text contents to be written to teh specified filepath</param>
        /// <param name="outputFilePath">An absolute filepath of where to write contents to.</param>
        /// <param name="defaultFileName">filename to be used if outputFilePath isnt defined</param>
        /// <returns>An absolute filepath of the written file.</returns>
        /// <exception cref="IOException"></exception>
        private static string WriteToFile<T>(T content, string outputFilePath, string defaultFileName)
        {
            string strJson = JsonConvert.SerializeObject(content, Formatting.Indented);
            if (string.IsNullOrWhiteSpace(outputFilePath))
            {
                //write the contents to the current directory with a default filename
                string curDirFilepath = Directory.GetCurrentDirectory() + "/" + defaultFileName;
                File.WriteAllText(curDirFilepath, strJson);
                
                return Path.GetFullPath(curDirFilepath);
                
            }

            File.WriteAllText(outputFilePath, strJson);
            return Path.GetFullPath(outputFilePath);
        }

        /// <summary>
        /// Checks a response for the Error type, throws an <see cref="APIException"/> with the message if true.
        /// </summary>
        /// <param name="restResponse">response to check contents for</param>
        /// <exception cref="APIException">The error message if it was an error.</returns>
        private static void CheckAPIError(IRestResponse restResponse)
        {
            try
            {
                APIError json = JsonConvert.DeserializeObject<APIError>(restResponse.Content);
                throw new APIException("API Error:" + json.Error);
            }
            catch (JsonException ex)
            {
                logger.Debug(ex);
                return; //probably not an Error json 
            }
        }

        /// <summary>
        /// unwraps the "json" array that appears in the schema of Guids and OsRegistryLocations.
        /// Without this step its impossible to determine the inner types for serialization.
        /// </summary>
        /// <param name="restResponse">Response from an API endpoint</param>
        /// <returns>a JSON array of unidentified objects</returns>
        private static string UnwrapJSONContainer(IRestResponse restResponse)
        {
            try
            {
               JArray array = JArray.Parse( JObject.Parse(restResponse.Content).GetValue("json").ToString());
               return array.ToString();
            } catch (JsonException ex)
            {
                throw new APIException("Error parsing API response.", ex);
            }
        }
    }

}
