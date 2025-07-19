using System;

namespace Khourse.Api.Dtos.ModuleDtos;

public class ModuleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CourseId { get; set; }
}
