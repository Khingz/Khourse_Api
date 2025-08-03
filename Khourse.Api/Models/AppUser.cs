using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Models;

public class AppUser : IdentityUser
{
    [Column("first_name")]
    public string? Firstname { get; set; }
    [Column("last_name")]
    public string? Lastname { get; set; }
    [Column("authored_courses")]
    public ICollection<Course> AuthoredCourses { get; set; } = [];
    public UserProfleSettings? UserProfile { get; set; }
    public ICollection<CourseEnrollment> CourseEnrollments { get; set; } = [];

}
