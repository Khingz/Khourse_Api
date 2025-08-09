using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.CourseDtos;

public class UpdateCourseRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public decimal? Price { get; set; }
    public CourseCategory? Category { get; set; }
}
