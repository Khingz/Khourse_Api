using System;
using Khourse.Api.Common;
using Khourse.Api.Data;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Helpers;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class CourseRepository(AppDbContext dbContext) : ICourseRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<PaginatedResponse<CourseDto>> GetAllAsync(CourseQueryOject query)
    {
        var courses = _dbContext.Course.Include(c => c.Modules).AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            courses = courses.Where(course => EF.Functions.ILike(course.Title, $"%{query.Title}%"));
        }
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            switch (query.SortBy.ToLower())
            {
                case "created_at":
                    courses = query.IsDecsending
                        ? courses.OrderByDescending(course => course.CreatedAt)
                        : courses.OrderBy(course => course.CreatedAt);
                    break;
                case "title":
                    courses = query.IsDecsending
                        ? courses.OrderByDescending(course => course.Title)
                        : courses.OrderBy(course => course.Title);
                    break;
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var totalItems = await courses.CountAsync();
        var result = await courses.OrderByDescending(c => c.CreatedAt).Skip(skipNumber).Take(query.PageSize).ToListAsync();
        var dtoData = result.Select(c => c.ToCourseDto()).ToList();
        var response = new PaginatedResponse<CourseDto>
        {
            Data = dtoData,
            CurrentPage = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
        };
        return response;

    }

    public async Task<Course> CreateAsync(Course courseModel)
    {
        await _dbContext.Course.AddAsync(courseModel);
        await _dbContext.SaveChangesAsync();
        return courseModel;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        var course = await _dbContext.Course.Include(c => c.Modules).FirstOrDefaultAsync(i => i.Id == id);
        return course;
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
        var course = await _dbContext.Course.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Course not found!");
        UpdateCourseFields(course, courseUpdateDto);
        await _dbContext.SaveChangesAsync();
        return course;
    }

    public async Task<Course?> DeleteAsync(Guid id)
    {
        var course = await _dbContext.Course.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Course not found!");
        _dbContext.Course.Remove(course);
        await _dbContext.SaveChangesAsync();
        return course;

    }

    public async Task<PaginatedResponse<ModuleDto>> GetCourseModulesAsync(Guid courseId, QueryObject queryObj)
    {
        var query = _dbContext.Module
        .Where(m => m.CourseId == courseId);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)queryObj.PageSize);
        var skipNumber = (queryObj.PageNumber - 1) * queryObj.PageSize;

        var modules = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skipNumber)
            .Take(queryObj.PageSize)
            .ToListAsync();

        var dtoData = modules.Select(c => c.ToModuleDto()).ToList();
        var response = new PaginatedResponse<ModuleDto>
        {
            Data = dtoData,
            CurrentPage = queryObj.PageNumber,
            PageSize = queryObj.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
        return response;
    }
}
