using Khourse.Api.Dtos.LessonDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface ILessonRepository
{
    Task<List<Lesson>> GetAllAsync(Guid moduleId);
    Task<Lesson> CreateAsync(Lesson lesson);
    Task<Lesson?> GetByIdAsync(Guid lessonId, Guid moduleId);
    Task<Lesson?> UpdateAsync(Guid id, UpdateLessonRequestDto lessonDto, Guid moduleId);
    Task<Lesson?> DeleteAsync(Guid id, Guid moduleId);
    Task<Lesson?> LessonByIdAsync(Guid lessonId);
}
