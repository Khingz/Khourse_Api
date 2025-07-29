using System.ComponentModel.DataAnnotations;

namespace Khourse.Api.Dtos.ModuleDtos;

public class CreateModuleDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Content is required")]
    [StringLength(10000, MinimumLength = 3, ErrorMessage = "Content must be between 3 and 10000 characters")]
    public string Content { get; set; } = string.Empty;
    public string? VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course Id is required and should be valid format")]
    public Guid? CourseId { get; set; }
}
