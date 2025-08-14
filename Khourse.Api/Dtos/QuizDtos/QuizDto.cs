using Khourse.Api.Models;

namespace Khourse.Api.Dtos.QuizDtos;

public class QuizDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;

    public int PassingScore { get; set; }

    public int TimeLimitMins { get; set; }

    public bool IsPublished { get; set; } = false;

    public string? Grade { get; set; }
    public Guid ModuleId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<QuizQuestion> Quizquestions { get; set; } = [];
}
