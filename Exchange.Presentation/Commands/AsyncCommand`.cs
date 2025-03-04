using System.Windows.Input;

namespace Exchange.Presentation.Commands
{
    public class AsyncCommand<TParam> : IAsyncCommand<TParam>
    {
        private readonly Func<TParam, CancellationToken, Task> _executionAction;
        private readonly AsyncCommandCancel _cancelCommand;
        private readonly AsyncExecutionState _state;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }

        public ICommand CancelCommand => _cancelCommand;

        public AsyncExecutionState State => _state;

        public AsyncCommand(Func<TParam, CancellationToken, Task> execution)
        {
            _executionAction = execution;
            _state = new AsyncExecutionState();
            _cancelCommand = new AsyncCommandCancel(_state);
        }

        public bool CanExecute(object? parameter)
        {
            return State.HasTask == false;
        }

        public async void Execute(object? parameter)
        {
            if (parameter is not TParam param)
                throw new Exception();

            await ExecuteAsync(param);
        }

        private async Task ExecuteAsync(TParam param)
        {
            CommandManager.InvalidateRequerySuggested();
            var token = _cancelCommand.StartExecute();

            await _state.StartProcessAsync(_executionAction.Invoke(param, token));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}