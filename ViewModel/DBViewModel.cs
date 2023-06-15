using System.Collections.Generic;
using System.ComponentModel;

namespace CrossesAndNoughts.ViewModel;

public class DBViewModel : INotifyPropertyChanged
{
    private IRecord _recordsProxy;

    public List<UserRecord> Records { get => _recordsProxy.GetRecords(); set => NotifyPropertyChanged("AllRecords"); }
    public DBViewModel()
    {
        using (IRecord records = new UserRecordsProxy())
        {
            _recordsProxy = records;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
