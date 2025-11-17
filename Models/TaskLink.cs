using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GraphManager.Models
{
    public class TaskLink : INotifyPropertyChanged
    {
        private TaskBlock _sourceBlock;
        private TaskBlock _targetBlock;

        public TaskLink()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        // 1. ДАННЫЕ ДЛЯ СОХРАНЕНИЯ (JSON)

        public string SourceBlockId { get; set; }
        public string TargetBlockId { get; set; }


        // 2. ДАННЫЕ ДЛЯ ОТОБРАЖЕНИЯ (WPF / XAML)

        [JsonIgnore]
        public TaskBlock SourceBlock
        {
            get => _sourceBlock;
            set
            {
                _sourceBlock = value;
                if (_sourceBlock != null) SourceBlockId = _sourceBlock.Id;
                OnPropertyChanged(nameof(SourceBlock));
            }
        }

        [JsonIgnore]
        public TaskBlock TargetBlock
        {
            get => _targetBlock;
            set
            {
                _targetBlock = value;
                if (_targetBlock != null) TargetBlockId = _targetBlock.Id;
                OnPropertyChanged(nameof(TargetBlock));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}