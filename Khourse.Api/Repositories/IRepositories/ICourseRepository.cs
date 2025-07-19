using System;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
    Task<Course> CreateAsync(Course course);
    Task<Course?> GetByIdAsync(Guid id);

}
