using System;

namespace Khourse.Api.Dtos.ModuleDtos;

public class UpdateModuleRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? VideoUrl { get; set; } = string.Empty;
    public Guid? CourseId { get; set; }
}
