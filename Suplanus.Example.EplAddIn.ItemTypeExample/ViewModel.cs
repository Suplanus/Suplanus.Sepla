using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Suplanus.Example.EplAddIn.ItemTypeExample
{
    public class ViewModel : INotifyPropertyChanged
    {
        private bool _isReadonly;

        public bool IsReadOnly
        {
            get => _isReadonly;
            set
            {
                if (value == _isReadonly) return;
                _isReadonly = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}