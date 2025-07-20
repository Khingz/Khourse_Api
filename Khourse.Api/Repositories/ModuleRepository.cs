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

    public Task<Module?> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Module>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Module?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Module.FirstOrDefaultAsync(i => i.Id == id);
    }

    public Task<Module?> UpdateAsync(Guid id, UpdateModuleRequestDto module)
    {
        throw new NotImplementedException();
    }
}
