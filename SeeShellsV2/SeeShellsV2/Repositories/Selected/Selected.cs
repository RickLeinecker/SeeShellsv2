using System;
using System.Collections.Generic;
using System.ComponentModel;

using Unity;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    public class Selected : ISelected
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public object CurrentInspector
        {
            get => _currentInspector;
            set
            {
                _currentInspector = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentInspector)));
            }
        }

        private object _currentInspector;

        public object CurrentData
        {
            get => _currentData;
            set
            {
                _currentData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentData)));
            }
        }

        private object _currentData;
    }
}
