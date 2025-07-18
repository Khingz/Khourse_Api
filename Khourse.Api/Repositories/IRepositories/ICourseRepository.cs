using System;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();

}
