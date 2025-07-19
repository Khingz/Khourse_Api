using System;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class ModuleMapper
{
    public static ModuleDto ToModuleDto(this Module module)
    {
        return new ModuleDto
        {
            Id = module.Id,
            Title = module.Title  
        };
    }

    public static Module ToModuleEntity(ModuleDto courseDto)
    {
        return new Module
        {

        };
    }
}
