using System;
using System.ComponentModel.DataAnnotations;
using Khourse.Api.Models;

namespace Khourse.Api.Dtos.CourseDtos;

public class CreateCourseRequestDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is Required")]
    public string Description { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;

    public bool IsPublished { get; set; }
}
