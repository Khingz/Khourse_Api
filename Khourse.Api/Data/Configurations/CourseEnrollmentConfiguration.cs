using Khourse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Khourse.Api.Data.Configurations;

public class CourseEnrollmentConfiguration : IEntityTypeConfiguration<CourseEnrollment>
{
    public void Configure(EntityTypeBuilder<CourseEnrollment> builder)
    {
        builder.HasKey(ce => ce.Id);

        builder.HasOne(ce => ce.Courses)
            .WithMany(c => c.CourseEnrollments)
            .HasForeignKey(ce => ce.CourseId);

        builder.HasOne(ce => ce.Students)
            .WithMany(s => s.CourseEnrollments)
            .HasForeignKey(ce => ce.StudentId);
    }
}
