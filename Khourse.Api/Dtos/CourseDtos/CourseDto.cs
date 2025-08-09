using Khourse.Api.Dtos.Account;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.CourseDtos;
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;

    public CourseCategory Category { get; set; }
    public UserDto? Author { get; set; }
    public bool IsPublished { get; set; }
    public decimal Price { get; set; }
    public int TotalModule { get; set; }
    public int DurationMins { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<ModuleDto> Modules { get; set; } = [];
}
