using System;
using Khourse.Api.Data;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class CourseRepository(AppDbContext dbContext) : ICourseRepository
{
    private readonly AppDbContext _dbContext = dbContext;

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

    public Task<bool> CourseExists(Guid id)
    {
        return _dbContext.Course.AnyAsync(s => s.Id == id);
    }

    private static void UpdateCourseFields(Course course, UpdateCourseRequestDto updateDto)
    {
        course.IsPublished = updateDto.IsPublished;
        course.Title = updateDto.Title ?? course.Title;
        course.Description = updateDto.Description ?? course.Description;
        course.ThumbnailUrl = updateDto.ThumbnailUrl ?? course.ThumbnailUrl;
    }

    public async Task<Course?> UpdateAsync(Guid id, UpdateCourseRequestDto courseUpdateDto)
    {
        var course = await _dbContext.Course.FirstOrDefaultAsync(x => x.Id == id);

        if (course == null)
        {
            return null;
        }
        UpdateCourseFields(course, courseUpdateDto);
        await _dbContext.SaveChangesAsync();
        return course;
    }

    public async Task<Course?> DeleteAsync(Guid id)
    {
        var course = await _dbContext.Course.FirstOrDefaultAsync(x => x.Id == id);

        if (course == null)
        {
            return null;
        }
        _dbContext.Course.Remove(course);
        await _dbContext.SaveChangesAsync();
        return course;

    }
}
