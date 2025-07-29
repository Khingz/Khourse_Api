using System.ComponentModel.DataAnnotations.Schema;

namespace Khourse.Api.Models;

public class Module : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = String.Empty;

    [Column("content")]
    public string Content { get; set; } = String.Empty;

    [Column("video_url")]
    public string? VideoUrl { get; set; }

    [Column("course_id")]
    public Guid? CourseId { get; set; }

    [Column("course")]
    public Course? Course { get; set; }
    
}
