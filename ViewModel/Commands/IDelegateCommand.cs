using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrossesAndNoughts.ViewModel.Commands;

public interface IDelegateCommand : ICommand
{
    void RaiseCanExecuteChanged();
}
