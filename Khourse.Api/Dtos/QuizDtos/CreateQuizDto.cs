using System.ComponentModel.DataAnnotations;

namespace Khourse.Api.Dtos.QuizDtos;

public class CreateQuizDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pass score is required")]
    public int PassingScore { get; set; }

    [Required(ErrorMessage = "Time limit in minute is required")]
    public int TimeLimitMins { get; set; }
}
