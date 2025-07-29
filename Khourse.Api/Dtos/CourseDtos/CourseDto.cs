using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Dtos.CourseDtos;
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;

    public AppUser? Author { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<ModuleDto> Modules { get; set; } = [];
}
