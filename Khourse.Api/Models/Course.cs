using System.ComponentModel.DataAnnotations.Schema;
using Khourse.Api.Enums;

namespace Khourse.Api.Models;

[Table("courses")]
public class Course : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = String.Empty;

    [Column("description")]
    public string Description { get; set; } = String.Empty;

    [Column("thumbnail_url")]
    public string ThumbnailUrl { get; set; } = String.Empty;

    [Column("category")]
    public CourseCategory Category { get; set; } = CourseCategory.Programming;

    [Column("duration_mins")]
    public int DurationMins { get; set; } = 0;

    [Column("total_module")]
    public int TotalModule { get; set; } = 0;

    [Column("price")]
    public decimal Price { get; set; } = 0;

    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    // Defines relationship with module
    [Column("modules")]
    public ICollection<Module> Modules { get; set; } = [];

    // Defines relationship with user
    [Column("author")]
    public AppUser? Author { get; set; }

    [Column("author_id")]
    public string? AuthorId { get; set; }

    public ICollection<CourseEnrollment> CourseEnrollments { get; set; } = [];

}
