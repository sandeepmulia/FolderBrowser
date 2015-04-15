using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// Code ref http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #region Constructors
        public DelegateCommand(Action<object> execute_)
        {
            this._execute = execute_;
        }

        public DelegateCommand(Action<object> execute_, Predicate<object> canExecute_)
        {
            if (execute_ != null)
            {
                this._execute = execute_;
                this._canExecute = canExecute_;
            }
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
