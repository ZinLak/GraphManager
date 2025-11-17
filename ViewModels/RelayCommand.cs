using System;
using System.Windows.Input;

namespace GraphManager.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Проверяем можно ли выполнить команду
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute(parameter);
        }

        // Выполнение команды
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}