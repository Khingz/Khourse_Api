using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Data;

using Khourse.Api.Models;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Module> Module => Set<Module>();
    public DbSet<Course> Course => Set<Course>();
    public DbSet<User> User => Set<User>();

    // This overides/set default value for createdAt and updateAt property fields
    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseModel>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}