using System;

namespace Swftx.Wpf.Commands
{
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
}
