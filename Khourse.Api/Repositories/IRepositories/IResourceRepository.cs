using Khourse.Api.Dtos.ResourceDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface IResourceRepository
{
    Task<List<Resource>> GetAllAsync(Guid moduleId);
    Task<Resource> CreateAsync(Resource resource);
    Task<Resource?> GetByIdAsync(Guid resourceId, Guid moduleId);
    Task<Resource?> UpdateAsync(Guid id, UpdateResourceRequestDto resourceDto, Guid moduleId);
    Task<Resource?> DeleteAsync(Guid id, Guid moduleId);
    Task<Resource?> ResourceByIdAsync(Guid resourceId);
}
