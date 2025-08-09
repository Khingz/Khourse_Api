using Khourse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Khourse.Api.Data.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(q => q.Id);

        builder.HasMany(q => q.Questions)
            .WithOne(qq => qq.Quiz)
            .HasForeignKey(qq => qq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
