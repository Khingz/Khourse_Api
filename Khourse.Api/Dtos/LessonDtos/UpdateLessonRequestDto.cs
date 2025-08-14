using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.LessonDtos;

public class UpdateLessonRequestDto
{
    public string Title { get; set; } = string.Empty;

    public ContentType? ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public string? TextContent { get; set; }

    public int? DurationMins { get; set; }

    public int? Position { get; set; }
}
