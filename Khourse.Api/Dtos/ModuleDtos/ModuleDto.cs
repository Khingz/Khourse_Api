using Khourse.Api.Dtos.LessonDtos;
using Khourse.Api.Dtos.QuizDtos;
using Khourse.Api.Dtos.ResourceDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Dtos.ModuleDtos;

public class ModuleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Position { get; set; }
    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CourseId { get; set; }
    public ICollection<QuizDto> Quizzes { get; set; } = [];
    public ICollection<ResourceDto> Resources { get; set; } = [];
    public ICollection<LessonDto> Lessons { get; set; } = [];
}
