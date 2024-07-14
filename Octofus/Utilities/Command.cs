using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Octofus.Utilities
{
    public class Command : ICommand
    {
        private Action<object> ExecuteAction { get; set; }

        private Func<object, bool> CanExecuteFunction { get; set; }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            ExecuteAction = execute;
            CanExecuteFunction = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunction != null)
            {
                return CanExecuteFunction(parameter);
            }

            return true;
        }

        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }

        public void Refresh()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
