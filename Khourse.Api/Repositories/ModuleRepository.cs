using Khourse.Api.Data;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class ModuleRepository(AppDbContext dbContext) : IModuleRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Module> CreateAsync(Module module)
    {
        await _dbContext.Module.AddAsync(module);
        await _dbContext.SaveChangesAsync();
        return module;
    }

    public async Task<Module?> DeleteAsync(Guid id, Guid courseId)
    {
        var module = await _dbContext.Module.FirstOrDefaultAsync(x => x.Id == id && x.CourseId == courseId) ?? throw new KeyNotFoundException("Module not found!");
        _dbContext.Module.Remove(module);
        await _dbContext.SaveChangesAsync();
        return module;
    }

    public async Task<List<Module>> GetAllAsync(Guid courseId)
    {
        return await _dbContext.Module
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.Position)
            .ToListAsync() ?? throw new KeyNotFoundException("Module not found!");
    }

    public async Task<Module?> GetByIdAsync(Guid moduleId, Guid courseId)
    {
        var module = await _dbContext.Module.FirstOrDefaultAsync(m => m.Id == moduleId && m.CourseId == courseId) ?? throw new KeyNotFoundException("Module not found!");
        return module;
    }

    public async Task<Module?> UpdateAsync(Guid id, UpdateModuleRequestDto moduleUpdateDto, Guid courseId)
    {
        var module = await _dbContext.Module.FirstOrDefaultAsync(x => x.Id == id && x.CourseId == courseId) ?? throw new KeyNotFoundException("Module not found!");
        UpdateModuleFields(module, moduleUpdateDto);
        await _dbContext.SaveChangesAsync();
        return module;
    }

    private static void UpdateModuleFields(Module module, UpdateModuleRequestDto updateDto)
    {
        module.Title = string.IsNullOrWhiteSpace(updateDto.Title)
            ? module.Title
            : updateDto.Title;

        module.Position = updateDto.Position ?? module.Position;
        module.EstimatedDurationMins = updateDto.EstimatedDurationMins ?? module.EstimatedDurationMins;
    }
}
