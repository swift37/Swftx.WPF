using System;

namespace Swftx.Wpf.Commands
{
    internal class LambdaCommandAsync : Command
    {
        private readonly ActionAsync<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public LambdaCommandAsync(ActionAsync Execute, Func<bool> CanExecute = null)
            : this(async param => await Execute(), CanExecute is null ? (Func<object, bool>)null : param => CanExecute())
        {

        }

        public LambdaCommandAsync(ActionAsync<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }

        protected override bool CanExecute(object param) => _CanExecute?.Invoke(param) ?? true;

        protected override void Execute(object param) => _Execute(param);
    }
}
