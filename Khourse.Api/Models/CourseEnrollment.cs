using System.ComponentModel.DataAnnotations.Schema;
using Khourse.Api.Enums;

namespace Khourse.Api.Models;

[Table("course_enrollments")]
public class CourseEnrollment : BaseModel
{
    [Column("student_id")]
    public string? StudentId { get; set; }

    [Column("students")]
    public AppUser? Students { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }
    public Course? Courses { get; set; }
    public EnrollmentStatusEnum Status { get; set; }
}
