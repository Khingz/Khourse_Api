using Khourse.Api.Data;
using Khourse.Api.Dtos.ResourceDtos;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class ResourceRepository(AppDbContext dbContext) : IResourceRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Resource> CreateAsync(Resource resource)
    {
        await _dbContext.Resource.AddAsync(resource);
        await _dbContext.SaveChangesAsync();
        return resource;
    }

    public async Task<Resource?> DeleteAsync(Guid id, Guid moduleId)
    {
        var resource = await _dbContext.Resource.FirstOrDefaultAsync(x => x.Id == id && x.ModuleId == moduleId) ?? throw new KeyNotFoundException("Resource not found!");
        _dbContext.Resource.Remove(resource);
        await _dbContext.SaveChangesAsync();
        return resource;
    }

    public async Task<List<Resource>> GetAllAsync(Guid moduleId)
    {
        return await _dbContext.Resource
            .Where(l => l.ModuleId == moduleId)
            .ToListAsync();
    }

    public async Task<Resource?> GetByIdAsync(Guid resourceId, Guid moduleId)
    {
        var resource = await _dbContext.Resource.FirstOrDefaultAsync(r => r.Id == resourceId && r.ModuleId == moduleId) ?? throw new KeyNotFoundException("Resource not found!");
        return resource;
    }

    public async Task<Resource?> ResourceByIdAsync(Guid resourceId)
    {
        var resource = await _dbContext.Resource.FirstOrDefaultAsync(r => r.Id == resourceId) ?? throw new KeyNotFoundException("Resource not found!");
        return resource;
    }

    public async Task<Resource?> UpdateAsync(Guid id, UpdateResourceRequestDto resourceDto, Guid moduleId)
    {
        var resource = await _dbContext.Resource.FirstOrDefaultAsync(x => x.Id == id && x.ModuleId == moduleId) ?? throw new KeyNotFoundException("Resource not found!");
        UpdateResourceFields(resource, resourceDto);
        await _dbContext.SaveChangesAsync();
        return resource;
    }

    private static void UpdateResourceFields(Resource resource, UpdateResourceRequestDto updateDto)
    {
        resource.Title = string.IsNullOrWhiteSpace(updateDto.Title)
            ? resource.Title
            : updateDto.Title;
        resource.Description = string.IsNullOrWhiteSpace(updateDto.Description)
            ? resource.Description
            : updateDto.Description;
        resource.ResourceUrl = string.IsNullOrWhiteSpace(updateDto.ResourceUrl)
            ? resource.ResourceUrl
            : updateDto.ResourceUrl;
        resource.Type = updateDto.Type ?? resource.Type;
    }

}
