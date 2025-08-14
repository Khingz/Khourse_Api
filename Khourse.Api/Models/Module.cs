using System.ComponentModel.DataAnnotations.Schema;

namespace Khourse.Api.Models;


[Table("modules")]
public class Module : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = String.Empty;

    [Column("position")]
    public int Position { get; set; }

    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    [Column("course_id")]
    public Guid? CourseId { get; set; }

    [Column("course")]
    public Course? Course { get; set; }

    public ICollection<Quiz> Quizzes { get; set; } = [];
    
    public ICollection<Resource> Resources { get; set; } = [];

    public ICollection<Lesson> Lessons { get; set; } = [];
}
