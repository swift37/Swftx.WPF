using System.ComponentModel;

namespace Swftx.Wpf.Commands;

public class LambdaCommand : Command
{
    private readonly Action<object?>? _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public LambdaCommand(Action Execute, Func<bool>? CanExecute = null)
        : this(param => Execute(), CanExecute is null ? null : param => CanExecute())
    {
    }

    public LambdaCommand(Action<object?>? Execute, Func<object?, bool>? CanExecute = null)
    {
        _Execute = Execute;
        _CanExecute = CanExecute;
    }

    protected override bool CanExecute(object? param) => _CanExecute?.Invoke(param) ?? true;

    protected override void Execute(object? param)
    {
        if (_Execute is null) throw new InvalidOperationException("Command execution method not defined");
        if (!CanExecute(param)) return;

        try
        {
            _Execute(param);
        }
        catch (Exception)
        {
            throw;
        }
    }
}

public class LambdaCommand<T> : Command
{
    private readonly Action<T?>? _Execute;
    private readonly Func<T?, bool>? _CanExecute;

    public LambdaCommand(Action<T?> Execute, Func<bool>? CanExecute)
        : this(Execute, CanExecute is null ? null : new Func<T?, bool>(_ => CanExecute()))
    {
    }

    public LambdaCommand(Action<T?> Execute, Func<T?, bool>? CanExecute = null)
    {
        _Execute = Execute;
        _CanExecute = CanExecute;
    }

    public static T? ConvertParameter(object? param)
    {
        switch (param)
        {
            case null: return default!;
            case T result: return result;
        }

        var commandType = typeof(T?);
        var paramType = param.GetType();

        if (commandType.IsAssignableFrom(paramType))
            return (T?)param;

        var commandTypeConv = TypeDescriptor.GetConverter(commandType);
        if (commandTypeConv.CanConvertFrom(paramType))
            return ((T?)commandTypeConv.ConvertFrom(param))!;

        var paramConv = TypeDescriptor.GetConverter(paramType);
        if (paramConv.CanConvertTo(commandType))
            return (T?)paramConv.ConvertFrom(param)!;

        return default!;
    }

    protected override bool CanExecute(object? param)
    {
        var canExecute = _CanExecute;
        if (canExecute is null) return true;
        if (param is null || param is T parameter && canExecute(parameter)) return true;
        param = ConvertParameter(param);
        return canExecute((T)param!);
    }

    protected override void Execute(object? param)
    {
        if (_Execute is null) throw new InvalidOperationException("Command execution method not defined");
        if (param != null && !(param is T))
            param = ConvertParameter(param);

        try
        {
            _Execute(ConvertParameter(param));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
