using System;
using System.Windows.Input;

namespace CrossesAndNoughts.ViewModel.Commands;

public class DelegateCommand : ICommand
{
    private readonly Predicate<object?>? _canExecute;
    private readonly Action<object?> _execute;

    public DelegateCommand(Action<object?> execute) : this(null, execute) { }

    private DelegateCommand(Predicate<object?>? canExecute, Action<object?> execute)
    {
        _canExecute = canExecute;
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}
