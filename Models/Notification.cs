using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GraphManager.Models
{
    public class Notification : INotifyPropertyChanged
    {
        private string _message;
        private bool _isVisible;

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; OnPropertyChanged(); }
        }

        public Notification(string message)
        {
            Message = message;
            IsVisible = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}