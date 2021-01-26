using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SeeShellsV2.UI
{
    public abstract class ViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
