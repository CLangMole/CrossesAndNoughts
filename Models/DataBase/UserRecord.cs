using System.ComponentModel.DataAnnotations;

namespace CrossesAndNoughts.Models.DataBase;

public class UserRecord(string userName, int place, int record)
{
    [Key]
    public string? UserName { get; set; } = userName;

    public int Place { get; set; } = place;
    public int Record { get; set; } = record;
}
