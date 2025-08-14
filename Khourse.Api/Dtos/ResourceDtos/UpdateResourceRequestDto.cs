using Khourse.Api.Enums;

namespace Khourse.Api.Dtos.ResourceDtos;

public class UpdateResourceRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ResourceTypeEnum? Type { get; set; }

    public string ResourceUrl { get; set; } = string.Empty;
}
