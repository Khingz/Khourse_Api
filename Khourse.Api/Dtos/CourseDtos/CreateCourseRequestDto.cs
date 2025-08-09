using System.ComponentModel.DataAnnotations;
using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.CourseDtos;

public class CreateCourseRequestDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is Required")]
    [StringLength(10000, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public decimal Price { get; set; }
    public CourseCategory Category { get; set; }
}
