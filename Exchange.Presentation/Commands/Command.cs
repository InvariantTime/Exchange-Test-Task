using System.Windows.Input;

namespace Exchange.Presentation.Commands
{
    public class Command : ICommand
    {
        private readonly Action _execution;

        public event EventHandler? CanExecuteChanged;

        public Command(Action execution)
        {
            _execution = execution;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execution.Invoke();
        }
    }
}
