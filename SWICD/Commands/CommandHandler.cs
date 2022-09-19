using System;
using System.Windows.Input;

namespace SWICD.Commands
{
    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<object> Action { get; set; }

        public CommandHandler(Action<object> action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action(parameter);
        }
    }
}