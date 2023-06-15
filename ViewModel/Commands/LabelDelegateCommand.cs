using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndNoughts.ViewModel.Commands;

public class LabelDelegateCommand : IDelegateCommand
{
    private Action<object> _execute;
    private Func<object, bool> _canExecute;

    public event EventHandler? CanExecuteChanged;

    public LabelDelegateCommand(Action<object> execute, Func<object, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public LabelDelegateCommand(Action<object> execute)
    {
        _execute = execute;
        _canExecute = AlwaysCanExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
       _canExecute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        if(CanExecuteChanged != null)
            CanExecuteChanged(this, EventArgs.Empty);
    }

    private bool AlwaysCanExecute(object parameter)
    {
        return true;
    }
}
