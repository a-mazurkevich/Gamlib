using System.Windows.Input;
using Gamlib.Extensions;

namespace Gamlib.Commands
{
    public abstract class BaseAsyncCommand : ICommand
    {
        protected bool IsExecuting;
        protected Action<Exception> ErrorHandler;

        public event EventHandler CanExecuteChanged;

        #region Implementation of ICommand

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        #endregion

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class AsyncCommand : BaseAsyncCommand, IAsyncCommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null, Action<Exception> errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            ErrorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return !IsExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    IsExecuting = true;
                    await _execute();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                finally
                {
                    IsExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        #region Override members of BaseAsyncCommand

        public override bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public override void Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(ErrorHandler);
        }

        #endregion
    }

    public class AsyncCommand<T> : BaseAsyncCommand, IAsyncCommand<T>
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, Action<Exception> errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            ErrorHandler = errorHandler;
        }

        public bool CanExecute(T parameter)
        {
            return !IsExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    IsExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    IsExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        #region Override members of BaseAsyncCommand

        public override bool CanExecute(object parameter)
        {
            if (parameter is T)
            {
                return CanExecute((T)parameter);
            }

            return false;
        }

        public override void Execute(object parameter)
        {
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync(ErrorHandler);
        }

        #endregion
    }
}

