using System;
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

    public async Task<Module?> DeleteAsync(Guid id)
    {
        var module = await _dbContext.Module.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Module not found!");
        _dbContext.Module.Remove(module);
        await _dbContext.SaveChangesAsync();
        return module;
    }

    public async Task<List<Module>> GetAllAsync()
    {
        return await _dbContext.Module.ToListAsync();
    }

    public async Task<Module?> GetByIdAsync(Guid id)
    {
        var module = await _dbContext.Module.FirstOrDefaultAsync(i => i.Id == id) ?? throw new KeyNotFoundException("Module not found!");
        return module;
    }

    public async Task<Module?> UpdateAsync(Guid id, UpdateModuleRequestDto moduleUpdateDto)
    {
        var module = await _dbContext.Module.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Module not found!");
        UpdateModuleFields(module, moduleUpdateDto);
        await _dbContext.SaveChangesAsync();
        return module;
    }

    private static void UpdateModuleFields(Module module, UpdateModuleRequestDto updateDto)
    {
        module.Title = updateDto.Title ?? module.Title;
        module.Content = updateDto.Content ?? module.Content;
        module.VideoUrl = updateDto.VideoUrl ?? module.VideoUrl;
    }
}
