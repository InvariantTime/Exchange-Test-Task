using System.Windows.Input;

namespace Exchange.Presentation.Commands
{
    public class AsyncCommand : IAsyncCommand
    {
        private readonly AsyncCommand<int> _base;

        public ICommand CancelCommand => _base.CancelCommand;

        public AsyncExecutionState State => _base.State;

        public event EventHandler? CanExecuteChanged;

        public AsyncCommand(Func<CancellationToken, Task> execution)
        {
            _base = new AsyncCommand<int>((_, c) => execution.Invoke(c));
            _base.CanExecuteChanged += (o, e) =>
            {
                CanExecuteChanged?.Invoke(o, e);
            };
        }

        public bool CanExecute(object? parameter)
        {
            return _base.CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _base.Execute(0);
        }
    }
}
