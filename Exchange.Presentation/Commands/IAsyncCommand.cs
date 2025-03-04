using System.Windows.Input;

namespace Exchange.Presentation.Commands
{
    public interface IAsyncCommand<T> : IAsyncCommand
    {
    }

    public interface IAsyncCommand : ICommand
    {
        ICommand CancelCommand { get; }

        AsyncExecutionState State { get; }
    }
}
