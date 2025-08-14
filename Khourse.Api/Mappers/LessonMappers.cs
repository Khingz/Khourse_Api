using Khourse.Api.Dtos.LessonDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class LessonMappers
{
    public static LessonDto ToLessonDto(this Lesson lesson)
    {
        return new LessonDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            ContentType = lesson.ContentType,
            ContentUrl = lesson.ContentUrl,
            TextContent = lesson.TextContent,
            DurationMins = lesson.DurationMins,
            Position = lesson.Position,
            IsPublished = lesson.IsPublished,
            ModuleId = lesson.ModuleId!.Value,
            CreatedAt = lesson.CreatedAt,
        };
    }

    public static Lesson ToLessonEntity(this CreateLessonDto lessonDto, Guid moduleId)
    {
        return new Lesson
        {
            Title = lessonDto.Title,
            ContentType = lessonDto.ContentType,
            ContentUrl = lessonDto.ContentUrl,
            TextContent = lessonDto.TextContent,
            DurationMins = lessonDto.DurationMins,
            Position = lessonDto.Position,
            ModuleId = moduleId
        };
    }
}
