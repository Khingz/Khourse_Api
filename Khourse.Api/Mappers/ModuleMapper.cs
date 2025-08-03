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
            Title = module.Title,
            CourseId = module.CourseId,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt ?? DateTime.UtcNow,
        };
    }

    public static Module ToModuleEntity(this CreateModuleDto moduleDto)
    {
        return new Module
        {
            Title = moduleDto.Title,
            CourseId = moduleDto.CourseId
        };
    }
}
