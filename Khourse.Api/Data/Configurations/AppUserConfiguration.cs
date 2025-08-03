using Khourse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Khourse.Api.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);

        // Defines relationship with courses user authored
        builder.HasMany(u => u.AuthoredCourses)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Defines relationship with profile settings
        builder.HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfleSettings>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
