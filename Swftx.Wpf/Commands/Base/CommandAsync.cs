using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Swftx.Wpf.Commands;

public abstract class CommandAsync : ICommand
{
    private bool _Executable = true;

    public bool Executable
    {
        get => _Executable;
        set
        {
            if (_Executable == value) return;
            _Executable = value;
            ExecutableChanged?.Invoke(this, EventArgs.Empty);
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public event EventHandler? ExecutableChanged;

    event EventHandler? ICommand.CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    bool ICommand.CanExecute(object? param) => _Executable && CanExecute(param);

    async void ICommand.Execute(object? param)
    {
        if (!((ICommand)this).CanExecute(param)) return;
        try
        {
            Executable = false;
            await ExecuteAsync(param);
        }
        catch
        {
            Executable = true;
            throw;
        }
    }

    protected virtual bool CanExecute(object? param) => true;

    protected abstract Task ExecuteAsync(object? param);
}