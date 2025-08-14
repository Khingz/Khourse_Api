using System.ComponentModel.DataAnnotations.Schema;
using Khourse.Api.Enums;

namespace Khourse.Api.Models;

[Table("lessons")]
public class Lesson : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("content_type")]
    public ContentType ContentType { get; set; }

    [Column("content_url")]
    public string? ContentUrl { get; set; }

    [Column("text_content")]
    public string? TextContent { get; set; }

    [Column("duration_mins")]
    public int? DurationMins { get; set; }

    [Column("position")]
    public int Position { get; set; }

    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    [Column("module_id")]
    public Guid? ModuleId { get; set; }

    [Column("module")]
    public Module? Module { get; set; }
}

