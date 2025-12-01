using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager.Models
{
    public class TaskBlock : INotifyPropertyChanged
    {
        private double _y;
        private double _x;
        private string _title = "Новая задача";
        private string _details = "Нажмите дважды для редактирования деталей...";
        private bool _isCompleted = false;
        private double _width = 150;
        private double _height = 80;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get => _title; set { _title = value; OnPropertyChanged(nameof(Title)); } }

        public double X { get => _x; set { _x = value; OnPropertyChanged(nameof(X)); } }
        public double Y { get => _y; set { _y = value; OnPropertyChanged(nameof(Y)); } }

        public double Width { get => _width; set { _width = value; OnPropertyChanged(nameof(Width)); } }
        public double Height { get => _height; set { _height = value; OnPropertyChanged(nameof(Height)); } }

        public string Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }
        public bool IsCompleted { get => _isCompleted; set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
