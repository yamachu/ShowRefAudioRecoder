using System;
using System.ComponentModel;

namespace AudioRecoder.Core.Models
{
    public class BaseNotifyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string parameterName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameterName));
    }
}
