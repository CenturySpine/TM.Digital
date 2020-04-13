using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TM.Digital.Ui.Resources.ViewModelCore
{
    public class NotifierBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}