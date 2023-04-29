namespace Swftx.Wpf.Commands;

public class LambdaCommandAsync : Command
{
    private readonly ActionAsync<object?>? _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public LambdaCommandAsync(ActionAsync? Execute, Func<bool>? CanExecute = null)
        : this(
              async param => await (Execute is null ? throw new ArgumentNullException(nameof(Execute)) : Execute()),
              CanExecute is null ? null : param => CanExecute())
    {
    }

    public LambdaCommandAsync(ActionAsync<object?>? Execute, Func<object?, bool>? CanExecute = null)
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
