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
            Position = module.Position,
            IsPublished = module.IsPublished,
            CourseId = module.CourseId,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt ?? DateTime.UtcNow,
            Quizzes = [.. module.Quizzes.Select(q => q.ToQuizDto())],
            Resources = [.. module.Resources.Select(r => r.ToResourceDto())],
            Lessons = [.. module.Lessons.Select(l => l.ToLessonDto())]
        };
    }

    public static Module ToModuleEntity(this CreateModuleDto moduleDto, Guid courseId)
    {
        return new Module
        {
            Title = moduleDto.Title,
            Position = moduleDto.Position,
            CourseId = courseId
        };
    }
}
