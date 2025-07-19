using System;
using Khourse.Api.Data;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _dbContext;

    public CourseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Course>> GetAllAsync()
    {
        return await _dbContext.Course.Include(c => c.Modules).ToListAsync();
    }

    public async Task<Course> CreateAsync(Course courseModel)
    {
        await _dbContext.Course.AddAsync(courseModel);
        await _dbContext.SaveChangesAsync();
        return courseModel;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Course.Include(c => c.Modules).FirstOrDefaultAsync(i => i.Id == id);
    }
}
