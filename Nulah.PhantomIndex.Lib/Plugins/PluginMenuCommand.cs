using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    public class PluginMenuCommand : ICommand
    {
        private readonly Action<string>? _commandAction;
        private readonly Func<bool>? _canExecute;

        public PluginMenuCommand(Action<string>? commandAction, Func<bool>? canExecute)
        {
            _commandAction = commandAction;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return false;
            }

            return _canExecute.Invoke();
        }

        void ICommand.Execute(object? parameter)
        {
            if (parameter is string navigationTarget)
            {
                _commandAction?.Invoke(navigationTarget);
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
