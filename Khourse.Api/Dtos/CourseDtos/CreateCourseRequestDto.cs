using System;
using Khourse.Api.Models;

namespace Khourse.Api.Dtos.CourseDtos;

public class CreateCourseRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;

    public bool IsPublished { get; set; }
}
