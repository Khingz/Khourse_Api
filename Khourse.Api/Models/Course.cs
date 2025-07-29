using System.ComponentModel.DataAnnotations.Schema;

namespace Khourse.Api.Models;

public class Course : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = String.Empty;

    [Column("description")]
    public string Description { get; set; } = String.Empty;

    [Column("thumbnail_url")]
    public string ThumbnailUrl { get; set; } = String.Empty;

    [Column("author")]
    public AppUser? Author { get; set; }

    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    [Column("modules")]
    public ICollection<Module> Modules { get; set; } = [];

}
