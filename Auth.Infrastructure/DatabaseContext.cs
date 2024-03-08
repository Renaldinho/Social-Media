using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext: DbContext
{
    public DbSet<User> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}