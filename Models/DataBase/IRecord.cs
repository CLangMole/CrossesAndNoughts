using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossesAndNoughts.Models.DataBase;

internal interface IRecord : IDisposable
{
    UserRecord GetRecord(int number);
    void AddRecord(UserRecord record);
    List<UserRecord> GetRecords();
}

internal class UserRecordsCollection : IRecord
{
    private readonly ApplicationContext _dataBase = new();

    public UserRecord GetRecord(int number)
    {
        var record = _dataBase.Records.FirstOrDefault(x => x.Place == number);

        return record ?? throw new IndexOutOfRangeException();
    }

    public List<UserRecord> GetRecords()
    {
        return _dataBase.Records.ToList();
    }

    public void Dispose()
    {
        _dataBase.Dispose();
    }

    public void AddRecord(UserRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        _dataBase.Records.Add(record);
        _dataBase.SaveChanges();
    }
}

public class UserRecordsProxy : IRecord
{
    private List<UserRecord> _records;
    private UserRecordsCollection _recordsCollection;

    public UserRecordsProxy()
    {
        _records = new List<UserRecord>();

        _recordsCollection ??= new UserRecordsCollection();
    }

    public UserRecord GetRecord(int number)
    {
        var record = _records.FirstOrDefault(x => x.Place == number);

        if (record is not null)
        {
            return record;
        }

        record = _recordsCollection.GetRecord(number);
        _records.Add(record);

        return record;
    }

    public List<UserRecord> GetRecords()
    {
        if (_records.Count != 0)
        {
            return _records;
        }
        
        _records = _recordsCollection.GetRecords();

        return _records;
    }

    public void Dispose()
    {
        _recordsCollection.Dispose();
        GC.SuppressFinalize(this);
    }

    public void AddRecord(UserRecord record)
    {
        if (_records.Count == 0)
        {
            _recordsCollection = new UserRecordsCollection();
        }

        _recordsCollection.AddRecord(record);
    }
}
