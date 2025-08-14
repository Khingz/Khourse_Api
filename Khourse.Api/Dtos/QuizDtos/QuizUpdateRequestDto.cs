namespace Khourse.Api.Dtos.QuizDtos;

public class QuizUpdateRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int? PassingScore { get; set; }
    public int? TimeLimitMins { get; set; }
    public string? Grade { get; set; }
}
