using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class CourseMapper
{
    public static CourseDto ToCourseDto(this Course course, IList<string> role)
    {
        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Author = course.Author?.ToUserDto(role),
            Category = course.Category,
            DurationMins = course.DurationMins,
            Price = course.Price,
            IsPublished = course.IsPublished,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt ?? DateTime.UtcNow,
            Modules = [.. course.Modules.Select(c => c.ToModuleDto())]
        };
    }

    public static Course ToCourseEntity(this CreateCourseRequestDto courseDto, string userId)
    {
        return new Course
        {
            Title = courseDto.Title,
            Description = courseDto.Description,
            ThumbnailUrl = courseDto.ThumbnailUrl,
            IsPublished = courseDto.IsPublished,
            Category = courseDto.Category,
            Price = courseDto.Price,
            AuthorId = userId
        };
    }

}
