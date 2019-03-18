using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleApp.Infrastructure
{
    class RelayCommand : ICommand
    {
        Action<object> action;
        Predicate<object> predicate;

        public RelayCommand(Action<object> act, Predicate<object> pred = null)
        {
            action = act;
            predicate = pred;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => predicate != null ? predicate(parameter) : true;

        public void Execute(object parameter) => action(parameter);
    }
}
