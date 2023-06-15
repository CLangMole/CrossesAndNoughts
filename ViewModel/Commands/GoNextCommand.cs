using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrossesAndNoughts.Models.Commandsl;
public class GoNextCommand : ICommand
{
    private Action _executeMethod;
    public event EventHandler? CanExecuteChanged;

    public GoNextCommand(Action executeMethod)
    {
        _executeMethod = executeMethod;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (_executeMethod != null) _executeMethod();
    }
}
