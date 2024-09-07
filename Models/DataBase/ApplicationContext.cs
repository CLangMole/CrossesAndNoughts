using Microsoft.EntityFrameworkCore;

namespace CrossesAndNoughts.Models.DataBase;

internal sealed class ApplicationContext : DbContext
{
    public DbSet<UserRecord> Records { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=tictactoedatabase;Trusted_Connection=True;");
    }
}
