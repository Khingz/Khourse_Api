using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khourse.Api.Models;

[Table("quizzes")]
public class Quiz : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("instructions")]
    public string Instructions { get; set; } = string.Empty;

    [Column("passing_score")]
    public int PassingScore { get; set; }

    [Column("time_limit_mins")]
    public int TimeLimitMins { get; set; }

    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    [Column("grade")]
    public string? Grade { get; set; }

    [Column("module_id")]
    public Guid? ModuleId { get; set; }

    [Column("module")]
    public Module? Module { get; set; }

    [Column("questions")]
    public ICollection<QuizQuestion> Questions { get; set; } = [];
}
