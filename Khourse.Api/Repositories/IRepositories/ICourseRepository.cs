using System;
using Khourse.Api.Common;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Helpers;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface ICourseRepository
{
    Task<PaginatedResponse<Course>> GetAllAsync(CourseQueryOject query);
    Task<Course> CreateAsync(Course course);
    Task<Course?> GetByIdAsync(Guid id);
    Task<Course?> UpdateAsync(Guid id, UpdateCourseRequestDto course);
    Task<Course?> DeleteAsync(Guid id);
    Task<bool> CourseExists(Guid id);
}
