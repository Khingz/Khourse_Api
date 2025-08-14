using Khourse.Api.Dtos.ResourceDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class ResourceMappers
{
    public static ResourceDto ToResourceDto(this Resource resource)
    {
        return new ResourceDto
        {
            Id = resource.Id,
            Title = resource.Title,
            Description = resource.Description,
            Type = resource.Type,
            ResourceUrl = resource.ResourceUrl,
            CreatedAt = resource.CreatedAt,
            ModuleId = resource.ModuleId
        };
    }

    public static Resource ToResourceEntity(this CreateResourceDto resourceDto, Guid moduleId)
    {
        return new Resource
        {
            Title = resourceDto.Title,
            Description = resourceDto.Description,
            Type = resourceDto.Type,
            ResourceUrl = resourceDto.ResourceUrl,
            ModuleId = moduleId
        };
    }
}
