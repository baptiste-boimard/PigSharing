using Microsoft.EntityFrameworkCore;
using PigSharing.Server.Database.Models;

namespace PigSharing.Server.Database;

public class PostgresDbContext: DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasKey(x => x.ConnectionToken);
    }
    
    
}