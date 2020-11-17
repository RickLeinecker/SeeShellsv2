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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI.ViewModels
{
    public class FileLocations : INotifyPropertyChanged
    {
        private string osLocation = string.Empty;
        private string guidLocation = string.Empty;
        private string scriptLocation = string.Empty;
        private string[] offlineLocations = new string[] { };

        public event PropertyChangedEventHandler PropertyChanged;

        public string OSFileLocation
        {
            get { return osLocation; }
            set
            {
                if (osLocation != value)
                {
                    osLocation = value;
                    OnPropertyChanged("OSFileLocation");
                }
            }
        }

        public string GUIDFileLocation
        {
            get { return guidLocation; }
            set
            {
                if (guidLocation != value)
                {
                    guidLocation = value;
                    OnPropertyChanged("GUIDFileLocation");
                }
            }
        }

        public string ScriptFileLocation
        {
            get { return scriptLocation; }
            set
            {
                if (scriptLocation != value)
                {
                    scriptLocation = value;
                    OnPropertyChanged("ScriptFileLocation");
                }
            }
        }

        public string[] OfflineFileLocations
        {
            get
            { 

                return offlineLocations; 
            }
            set
            {
                if (offlineLocations != value)
                {
                    offlineLocations = value;
                    OnPropertyChanged("OfflineFileLocations");
                }
            }
        }

        public FileLocations(string os, string guid, string script)
        {
            OSFileLocation = os;
            guidLocation = guid;
            scriptLocation = script;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
