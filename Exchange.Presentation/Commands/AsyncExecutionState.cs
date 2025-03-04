using System.ComponentModel;
using System.Threading.Tasks;

namespace Exchange.Presentation.Commands
{
    public class AsyncExecutionState : INotifyPropertyChanged
    {
        private Task? _currentTask;

        public bool IsCompleted => _currentTask?.IsCompleted ?? false;

        public bool HasTask => _currentTask != null && IsCompleted == false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task StartProcessAsync(Task task)
        {
            _currentTask = task;
            NotifyChanged();

            await task;

            NotifyChanged();
        }

        private void NotifyChanged()
        {
            OnPropertyChanged(nameof(HasTask));
            OnPropertyChanged(nameof(IsCompleted));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}