using Microsoft.EntityFrameworkCore;
using PigSharing.Server.Models;


namespace PigSharing.Server.Database;

public class PostgresDbContext: DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Picture> Pictures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.ConnectionToken);
            entity.Property(a => a.UserName).IsRequired();
            entity.Property(a => a.Password).IsRequired();
            entity.HasMany(a => a.Pictures)
                .WithOne(a => a.Account)
                .HasForeignKey(p => p.AccountId);
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(a => a.Account)
                .WithMany(p => p.Pictures)
                .HasForeignKey(p => p.AccountId);
        });
    }
}