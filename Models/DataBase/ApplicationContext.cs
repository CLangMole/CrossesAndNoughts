using Microsoft.EntityFrameworkCore;

namespace CrossesAndNoughts.Models.DataBase;

internal sealed class ApplicationContext : DbContext
{
    public DbSet<UserRecord> Records { get; set; }

    public ApplicationContext()
    {
        Database.EnsureCreatedAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=crossesandnoughtsdb;Trusted_Connection=True;");
    }
}
