using System.ComponentModel.DataAnnotations;
using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.ResourceDtos;

public class CreateResourceDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 100 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Resource type is required")]
    public ResourceTypeEnum Type { get; set; }

    [Required(ErrorMessage = "Resource URL is required")]
    public string ResourceUrl { get; set; } = string.Empty;
}
