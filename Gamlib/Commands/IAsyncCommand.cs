using System.Windows.Input;

namespace Gamlib.Commands;

public interface IAsyncCommand<T> : ICommand
{
    Task ExecuteAsync(T parameter);

    bool CanExecute(T parameter);
}

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync();

    bool CanExecute();
}

