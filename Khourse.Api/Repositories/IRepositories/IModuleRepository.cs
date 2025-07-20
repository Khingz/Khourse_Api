using System;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface IModuleRepository
{
    Task<List<Module>> GetAllAsync();
    Task<Module> CreateAsync(Module module);
    Task<Module?> GetByIdAsync(Guid id);
    Task<Module?> UpdateAsync(Guid id, UpdateModuleRequestDto moduleDto);
    Task<Module?> DeleteAsync(Guid id);
}
