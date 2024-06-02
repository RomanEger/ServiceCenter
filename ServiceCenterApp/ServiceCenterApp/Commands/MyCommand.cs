using System.Windows.Input;

namespace ServiceCenterApp.Commands
{
    public class MyCommand : ICommand
    {
        private Action execute;
        private bool canExecute = false;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public MyCommand(Action execute, bool canExecute = true)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? _ = null) => canExecute;

        public void Execute(object? _ = null) => execute();
    }
}
