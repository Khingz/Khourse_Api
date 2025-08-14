using System;
using System.ComponentModel.DataAnnotations;
using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.LessonDtos;

public class CreateLessonDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content type is required - Text, Slides, Video")]
    public ContentType ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public string? TextContent { get; set; }

    [Required(ErrorMessage = "Lesson duration is required")]
    public int? DurationMins { get; set; }

    [Required(ErrorMessage = "Lesson position in module is required")]
    public int Position { get; set; }

}
