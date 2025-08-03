using System.ComponentModel.DataAnnotations.Schema;
using Khourse.Api.Enums;

namespace Khourse.Api.Models;

[Table("quiz_questions")]
public class QuizQuestion : BaseModel
{
    [Column("question_text")]
    public string QuestionText { get; set; } = string.Empty;

    [Column("question_type")]
    public QuestionTypeEnum QuestionType { get; set; }

    [Column("options")]
    public string Options { get; set; } = string.Empty;

    [Column("correct_answer")]
    public string CorrectAnswer { get; set; } = string.Empty;

    [Column("explanation")]
    public string Explanation { get; set; } = string.Empty;

    [Column("quiz_id")]
    public Guid QuizId { get; set; }

    [Column("quiz")]
    public Quiz? Quiz { get; set; }
}
