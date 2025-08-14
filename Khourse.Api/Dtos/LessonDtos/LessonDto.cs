using System;
using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.LessonDtos;

public class LessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public ContentType ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public string? TextContent { get; set; }

    public int? DurationMins { get; set; }

    public int Position { get; set; }

    public bool IsPublished { get; set; } = false;

    public Guid ModuleId { get; set; }
    public DateTime CreatedAt { get; set; }
}
