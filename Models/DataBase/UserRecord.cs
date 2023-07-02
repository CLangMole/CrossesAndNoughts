using System.ComponentModel.DataAnnotations;

namespace CrossesAndNoughts.Models.DataBase;

public class UserRecord
{
    [Key]
    public string? UserName { get; set; }
    public int Place { get; set; }
    public int Record { get; set; }
}
