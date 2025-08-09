using System;
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
        };
    }

    public static Resource ToResourceEntity(this CreateResourceDto resourceDto)
    {
        return new Resource
        {

        };
    }
}
