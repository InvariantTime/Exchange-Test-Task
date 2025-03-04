using System;
using System.Threading;
using System.Windows.Input;

namespace Exchange.Presentation.Commands
{
    class AsyncCommandCancel : ICommand
    {
        private readonly AsyncExecutionState _state;
        private CancellationTokenSource _cancellation;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncCommandCancel(AsyncExecutionState state)
        {
            _state = state;
            _cancellation = new CancellationTokenSource();
        }

        public bool CanExecute(object? parameter)
        {
            return _cancellation?.IsCancellationRequested == false && _state.HasTask == true;
        }

        public void Execute(object? parameter)
        {
            _cancellation?.Cancel();
            CommandManager.InvalidateRequerySuggested();
        }

        public CancellationToken StartExecute()
        {
            if (_cancellation.IsCancellationRequested == true)
            {
                _cancellation = new CancellationTokenSource();
                CommandManager.InvalidateRequerySuggested();
            }

            return _cancellation.Token;
        }
    }
}
