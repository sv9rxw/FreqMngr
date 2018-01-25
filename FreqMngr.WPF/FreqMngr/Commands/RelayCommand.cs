using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FreqMngr.Commands
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _Execute;
        readonly Predicate<object> _CanExecute;

        public RelayCommand(Action<object> execute)
            :this(execute, null)
        {

        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new NullReferenceException("execute");

            this._Execute = execute;
            this._CanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return (_CanExecute == null || _CanExecute(parameter));
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }
}
