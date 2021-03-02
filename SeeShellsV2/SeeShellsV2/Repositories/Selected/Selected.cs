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

        public object Current
        {
            get => _current;
            set
            {
                _current = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Current"));
            }
        }

        public IEnumerable<object> CurrentEnumerable
        {
            get => _currentEnumerable;
            set
            {
                _currentEnumerable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentEnumerable"));
            }
        }

        private object _current;
        private IEnumerable<object> _currentEnumerable;
    }
}
