using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CrossesAndNoughts;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel.Commands;

namespace CrossesAndNoughts.ViewModel
{
    public class DBViewModel : INotifyPropertyChanged
    {
        public ICommand? GoNextCommand { get => _goNextCommand; protected set => _goNextCommand = (IDelegateCommand)value; }
        private IDelegateCommand _goNextCommand;

        public DBViewModel()
        {
            _goNextCommand = new LabelDelegateCommand(ExecuteGoNext, CanExecuteGoNext);
        }

        private List<UserRecord> _records()
        {
            using (IRecord records = new UserRecordsProxy())
            {
                if (records.GetRecords().Count <= 0)
                    throw new Exception();
                return records.GetRecords();
            }
        }

        public List<UserRecord> Records
        {
            get { return _records(); }
            private set
            {
                Records = value;
                NotifyPropertyChanged(nameof(Records));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ExecuteGoNext(object parameters)
        {
            object[]? objects = parameters as object[];
            foreach (UIElement nextControl in (UIElement[])objects[1])
            {
                if (nextControl == null) return;
                nextControl.Visibility = Visibility.Visible;

                Grid? grid = objects[0] as Grid;

                if (grid?.Children.Count <= 0) return;
                foreach (UIElement childrenControl in grid.Children)
                {
                    if (childrenControl == nextControl || childrenControl.Uid == "CollapsedAtStart") continue;
                    childrenControl.Visibility = Visibility.Collapsed;
                }
            }
        }
        private bool CanExecuteGoNext(object parameter) => true;
    }
}
