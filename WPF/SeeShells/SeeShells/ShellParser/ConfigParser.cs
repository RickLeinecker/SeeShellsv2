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
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using SeeShells.UI.ViewModels;
using Microsoft.Win32;
using NLog;
using SeeShells.ShellParser.Scripting;
using SeeShells.UI.Pages;

namespace SeeShells.ShellParser
{
    class ConfigParser : IConfigParser
    {
        private readonly string OSRegistryFile;
        private const string DefaultOsConfig = "defaultOS.json";
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public string OsVersion {private get; set;}

        public ConfigParser(string guidsFile = "", string OSRegistryFile = "", string scriptsFile = "")
        {
            guidsFile = IsValidGuidFile(guidsFile) ? guidsFile : string.Empty;
            OSRegistryFile = IsValidOsFile(OSRegistryFile) ? OSRegistryFile : string.Empty;
            scriptsFile = IsValidScriptFile(scriptsFile) ? scriptsFile : string.Empty;

            UpdateKnownGUIDS(guidsFile);
            UpdateScripts(scriptsFile);
            OsVersion = getLiveOSVersion();
            this.OSRegistryFile = OSRegistryFile;
        }



        /// <summary>
        /// Overrides (or Adds) existing GUID entries into <see cref="KnownGuids.dict"/>
        /// </summary>
        /// <param name="guidsFile">A JSON file that contains <see cref="GUIDPair"/></param>
        private void UpdateKnownGUIDS(string guidsFile)
        {
            if (guidsFile.Equals(string.Empty))
                return;

            IList<GUIDPair> guidPairs = JsonConvert.DeserializeObject<IList<GUIDPair>>(File.ReadAllText(guidsFile));
            foreach(GUIDPair pair in guidPairs)
            {   
                KnownGuids.dict[pair.getKnownGUID().Key] = pair.getKnownGUID().Value;
            }

        }

        private void UpdateScripts(string file)
        {
            if (file.Equals(string.Empty))
                return;

            IList<DecodedScriptPair> scriptPairs = JsonConvert.DeserializeObject<IList<DecodedScriptPair>>(File.ReadAllText(file));
            foreach(DecodedScriptPair pair in scriptPairs)
            {
                ScriptHandler.scripts[pair.getScript().Key] = pair.getScript().Value;   
            }

        }


        public List<string> GetRegistryLocations()
        {
            List<string> locations = new List<string>();

            IList<RegistryLocations> registryLocations = OSRegistryFile.Equals(string.Empty)
                ? GetDefaultRegistryLocations()
                : JsonConvert.DeserializeObject<IList<RegistryLocations>>(File.ReadAllText(OSRegistryFile));

            foreach (RegistryLocations regLocation in registryLocations)
            {
                if (OsVersion.Contains(regLocation.OperatingSystem))
                {
                    foreach (IList<string> registryPaths in regLocation.GetRegistryFilePaths().Values)
                    {
                        locations.AddRange(registryPaths);
                    }
                    return locations;
                }
            }

            return locations;
        }

        public List<string> GetUsernameLocations()
        {
            //todo retrieve from OSRegistryFile instead of hardcoded path
            List<string> list = new List<string>();
            // found username retrievalable key-value @ https://stackoverflow.com/a/53585223
            list.Add(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");
            return list;
        }

        private string getLiveOSVersion()
        {
            RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            return (string)registryKey.GetValue("productName");   
        }


        /// <summary>
        /// Retrieves the default <see cref="RegistryLocations"/> configuration from the embedded resource.
        /// </summary>
        /// <returns>A list of Registry Locations.</returns>
        public static IList<RegistryLocations> GetDefaultRegistryLocations()
        {
            IList<RegistryLocations> retval = new List<RegistryLocations>();
            try
            {
                //internal resource retrieval, see: https://stackoverflow.com/a/3314213
                Assembly assembly = Assembly.GetExecutingAssembly();
                string internalResourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(DefaultOsConfig));
                using (Stream fileStream = assembly.GetManifestResourceStream(internalResourcePath))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        retval = JsonConvert.DeserializeObject<IList<RegistryLocations>>(reader.ReadToEnd());
                    }
                }
            }
            catch (JsonSerializationException)
            {
                //this should never happen. if it does, we are somehow missing internal (compiled) resources or its malformed.
                logger.Fatal("Cannot load default OS Configuration file. Internal resource is missing or broken." +
                             " Please notify the developers with your error log and browse for a new OS Configuration file.");
            }

            return retval;
        }

        /// <summary>
        /// Checks if a JSON file can be deserialized for a valid known Type
        /// </summary>
        /// <typeparam name="T">The known API Type to deserialize from. See <see cref="IO.Networking.JSON"/></typeparam>
        /// <param name="location">a fully qualified path to a json file</param>
        /// <returns>true, if the file could successfully be deserialized. False otherwise.</returns>
        private static bool IsValidConfigFile<T>(string location)
        {
            if (File.Exists(location))
            {
                string json = File.ReadAllText(location);
                try
                {
                    JsonConvert.DeserializeObject<T> (json);
                    //if we can deserialize, we can use it.
                    return true;
                }
                catch (JsonSerializationException ex)
                {
                    logger.Error(ex);
                    return false;
                }
            }

            return false;
        }

        public static bool IsValidOsFile(string location)
        {
            return IsValidConfigFile<IList<RegistryLocations>>(location);
        }
        public static bool IsValidGuidFile(string location)
        {
            return IsValidConfigFile<IList<GUIDPair>>(location);
        }

        public static bool IsValidScriptFile(string location)
        {
            return IsValidConfigFile<IList<DecodedScriptPair>>(location);
        }

    }
}
