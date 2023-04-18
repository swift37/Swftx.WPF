using System;
using System.Windows.Input;

namespace Swftx.Wpf.Commands
{
    internal abstract class Command : ICommand
    {
        private bool _executable = true;

        public bool Executable
        {
            get => _executable;
            set
            {
                if (_executable == value) return;
                _executable = value;
                ExecutableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ExecutableChanged;

        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        bool ICommand.CanExecute(object param) => _executable && CanExecute(param);

        void ICommand.Execute(object param)
        {
            if (!((ICommand)this).CanExecute(param)) return;
            Execute(param);
        }

        protected virtual bool CanExecute(object param) => true;

        protected abstract void Execute(object param);
    }
}
