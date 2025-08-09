using System.ComponentModel.DataAnnotations;

namespace Khourse.Api.Dtos.ModuleDtos;

public class CreateModuleDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Module position in course is required")]
    public int Position { get; set; }

    [Required(ErrorMessage = "Estimated mins is required is required")]
    public int EstimatedDurationMins { get; set; }

    [Required(ErrorMessage = "Course Id (course_id) is required and should be valid format")]
    public Guid? CourseId { get; set; }
}
