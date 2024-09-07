using System.ComponentModel.DataAnnotations;

namespace CrossesAndNoughts.Models.DataBase;

public class UserRecord(string userName, int playOrder, int record)
{
    [Key]
    [MaxLength(100)]
    public string? UserName { get; init; } = userName;

    public int PlayOrder { get; init; } = playOrder;
    public int Record { get; init; } = record;
}
