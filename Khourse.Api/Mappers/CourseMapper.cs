using System;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class CourseMapper
{
    public static CourseDto ToCourseDto(this Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Author = course.Author,
            IsPublished = course.IsPublished,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt ?? DateTime.UtcNow,
            Modules = [.. course.Modules.Select(c => c.ToModuleDto())]
        };
    }

    public static Course ToCourseEntity(this CreateCourseRequestDto courseDto)
    {
        return new Course
        {
            Title = courseDto.Title,
            Description = courseDto.Description,
            ThumbnailUrl = courseDto.ThumbnailUrl,
            IsPublished = courseDto.IsPublished,
        };
    }

}
