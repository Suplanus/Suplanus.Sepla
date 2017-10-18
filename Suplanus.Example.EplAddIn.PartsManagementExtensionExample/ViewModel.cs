using System.ComponentModel;
using System.Runtime.CompilerServices;
using Suplanus.Example.EplAddIn.PartsManagementExtensionExample.Annotations;

namespace Suplanus.Example.EplAddIn.PartsManagementExtensionExample
{
    public class ViewModel : INotifyPropertyChanged
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}