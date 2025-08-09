using Khourse.Api.Common;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Helpers;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface ICourseRepository
{
    Task<PaginatedResponse<CourseDto>> GetAllAsync(CourseQueryOject query);
    Task<CourseDto> CreateAsync(Course course);
    Task<CourseDto?> GetByIdAsync(Guid id);
    Task<CourseDto?> UpdateAsync(Guid id, UpdateCourseRequestDto courseUpdateDto, string currentUserID);
    Task<CourseDto?> DeleteAsync(Guid id, string currentUserID);
    Task<bool> CourseExists(Guid id);
    Task<PaginatedResponse<ModuleDto>> GetCourseModulesAsync(Guid courseId, QueryObject queryObj);
}
