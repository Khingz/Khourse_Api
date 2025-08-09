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
        };
    }

    public static Lesson ToLessonEntity(this CreateLessonDto lessonDto)
    {
        return new Lesson
        {

        };
    }
}
