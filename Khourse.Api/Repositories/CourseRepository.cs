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

public class CourseRepository(AppDbContext dbContext, IAccountRepository accountRepo) : ICourseRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IAccountRepository _accountRepo = accountRepo;

    public async Task<PaginatedResponse<CourseDto>> GetAllAsync(CourseQueryOject query)
    {
        var courses = _dbContext.Course.Include(c => c.Modules).Include(c => c.Author).AsQueryable();
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
        var dtoData = new List<CourseDto>();
        foreach (var course in result)
        {
            var user = await _accountRepo.UserByIdAsync(course.AuthorId!);

            var authorRoles = await _accountRepo.GetUserRolesAsync(user!);
            dtoData.Add(course.ToCourseDto([.. authorRoles]));
        }
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

    public async Task<CourseDto> CreateAsync(Course courseModel)
    {
        var user = await _accountRepo.UserByIdAsync(courseModel.AuthorId!) ?? throw new AccessViolationException("User in not authorized to create resource");
        await _dbContext.Course.AddAsync(courseModel);
        await _dbContext.SaveChangesAsync();
        var roles = await _accountRepo.GetUserRolesAsync(user!);
        return courseModel.ToCourseDto(roles);
    }

    public async Task<CourseDto?> GetByIdAsync(Guid id)
    {
        var course = await CourseById(id);
        var roles = await _accountRepo.GetUserRolesAsync(course!.Author!);
        return course.ToCourseDto(roles);
    }

    public async Task<Course?> CourseById(Guid id)
    {
        var course = await _dbContext.Course
            .Include(c => c.Author)
            .Include(c => c.Modules)
            .FirstOrDefaultAsync(i => i.Id == id) ?? throw new KeyNotFoundException("Course not found");
        return course;
    }


    public Task<bool> CourseExists(Guid id)
    {
        return _dbContext.Course.AnyAsync(s => s.Id == id);
    }

    private static void UpdateCourseFields(Course course, UpdateCourseRequestDto updateDto)
    {
        course.IsPublished = updateDto.IsPublished;

        course.Title = string.IsNullOrWhiteSpace(updateDto.Title)
            ? course.Title
            : updateDto.Title;

        course.Description = string.IsNullOrWhiteSpace(updateDto.Description)
            ? course.Description
            : updateDto.Description;

        course.ThumbnailUrl = string.IsNullOrWhiteSpace(updateDto.ThumbnailUrl)
            ? course.ThumbnailUrl
            : updateDto.ThumbnailUrl;

        course.Price = updateDto.Price ?? course.Price;
        course.Category = updateDto.Category ?? course.Category;
    }

    public async Task<CourseDto?> UpdateAsync(Guid id, UpdateCourseRequestDto courseUpdateDto, string currentUserID)
    {
        var course = await CourseById(id);
        ;
        var currentUser = await _accountRepo.UserByIdAsync(currentUserID!) ?? throw new UnauthorizedAccessException("You are not authorized to update this course"); ;
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && currentUserID != course!.AuthorId)
        {
            throw new AccessViolationException("You are not authorized to update this course");
        }
        UpdateCourseFields(course!, courseUpdateDto);
        await _dbContext.SaveChangesAsync();
        var roles = await _accountRepo.GetUserRolesAsync(course!.Author!);
        return course.ToCourseDto(roles);
    }

    public async Task<CourseDto?> DeleteAsync(Guid id, string currentUserID)
    {
        var course = await CourseById(id);
        var user = await _accountRepo.UserByIdAsync(currentUserID!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this course"); ;
        var isAdmin = await _accountRepo.UserHasRoleAsync(user, "Admin");
        if (!isAdmin && currentUserID != course!.AuthorId)
        {
            throw new AccessViolationException("You are not authorized to delete this course");
        }
        _dbContext.Course.Remove(course!);
        await _dbContext.SaveChangesAsync();
        var roles = await _accountRepo.GetUserRolesAsync(course!.Author!);
        return course.ToCourseDto(roles);
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
