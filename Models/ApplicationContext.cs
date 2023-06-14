using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossesAndNoughts;

class ApplicationContext : DbContext
{
    public DbSet<UserRecord> Records { get; set; }

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=crossesandnoughtsdb;Trusted_Connection=True;");
    }
}

interface IRecord : IDisposable
{
    UserRecord GetRecord(int number);
    List<UserRecord> GetRecords();
}

class UserRecordsCollection : IRecord
{
    private ApplicationContext _dataBase;
    public UserRecordsCollection()
    {
        _dataBase = new ApplicationContext();
    }

    public UserRecord GetRecord(int number)
    {
        return _dataBase.Records.FirstOrDefault(x => x.Place == number);
    }

    public List<UserRecord> GetRecords()
    {
        return _dataBase.Records.ToList();
    }

    public void Dispose()
    {
        _dataBase.Dispose();
    }
}

public class UserRecordsProxy : IRecord
{
    private List<UserRecord> _records;
    private UserRecordsCollection _recordsCollection;

    public UserRecordsProxy()
    {
        _records = new List<UserRecord>();
    }

    public UserRecord GetRecord(int number)
    {
        UserRecord record = _records.FirstOrDefault(x => x.Place == number);
        if (record == null)
        {
            if (_recordsCollection == null)
                _recordsCollection = new UserRecordsCollection();
            record = _recordsCollection.GetRecord(number);
            _records.Add(record);
        }
        return record;
    }

    public List<UserRecord> GetRecords()
    {
        if(_records.Count == 0)
        {
            if(_recordsCollection == null)
                _recordsCollection = new UserRecordsCollection();
            _records = _recordsCollection.GetRecords();
        }
        return _records;
    }

    public void Dispose()
    {
        if (_recordsCollection != null)
        {
            _recordsCollection.Dispose();
        }
    }
}
