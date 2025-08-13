using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface IModuleRepository
{
    Task<List<Module>> GetAllAsync(Guid courseId);
    Task<Module> CreateAsync(Module module);
    Task<Module?> GetByIdAsync(Guid moduleId, Guid courseId);
    Task<Module?> UpdateAsync(Guid id, UpdateModuleRequestDto moduleDto, Guid courseId);
    Task<Module?> DeleteAsync(Guid id, Guid courseId);
    Task<Module?> ModuleByIdAsync(Guid moduleId);
    
}
