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
using CrossesAndNoughts.Models;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel.Commands;

namespace CrossesAndNoughts.ViewModel
{
    public class DBViewModel : INotifyPropertyChanged
    {
        public DelegateCommand GoNextCommand { get => _goNextCommand; }

        private DelegateCommand _goNextCommand = new DelegateCommand(ClickMethods.GoNext);

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
    }
}
