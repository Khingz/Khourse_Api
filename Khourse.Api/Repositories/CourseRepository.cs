using System;
using Khourse.Api.Data;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _dbContext;

    public CourseRepository(AppDbContext dbContext) {
        _dbContext = dbContext;
    }
    public async Task<List<Course>> GetAllAsync()
    {
        var stocks = await _dbContext.Course.ToListAsync<Course>();
        return stocks;
    }
}
