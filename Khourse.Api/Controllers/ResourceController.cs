using Khourse.Api.Common;
using Khourse.Api.Dtos.ResourceDtos;
using Khourse.Api.Filters;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/modules/{moduleId}/resources")]
[ServiceFilter(typeof(ModuleExistFilter))]
public class ResourceController(ICurrentUserService currentUserService, IAccountRepository accountRepo, IResourceRepository resourceRepo) : BaseController
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly IResourceRepository _resourceRepo = resourceRepo;


    [Authorize(Roles = "Author,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateResource(Guid moduleId, [FromBody] CreateResourceDto resourceDto)
    {
        var module = HttpContext.Items["Module"] as Module;
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to add module to this course");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to add resource to this module");
        }
        var resourceModel = resourceDto.ToResourceEntity(module!.Id);
        await _resourceRepo.CreateAsync(resourceModel);
        var response = ApiSuccessResponse<ResourceDto>.Ok(resourceModel.ToResourceDto(), "Resource created successfully");
        return CreatedAtAction(nameof(GetById), new { moduleId = moduleId, id = resourceModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid resourceGuid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var resource = await _resourceRepo.GetByIdAsync(resourceGuid, moduleId);
        return OkResponse("Resource feteched successfully", resource!.ToResourceDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourseModules(Guid moduleId)
    {
        var resource = await _resourceRepo.GetAllAsync(moduleId);
        var resourceDto = resource.Select(l => l.ToResourceDto());
        return OkResponse("Resources fetched successfully", resourceDto);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var resource = await _resourceRepo.GetByIdAsync(guid, moduleId) ?? throw new KeyNotFoundException("Resource not found");
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this Lesson");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this resource");
        }
        await _resourceRepo.DeleteAsync(guid, moduleId);
        return OkResponse("Resource deleted successfully", resource.ToResourceDto());
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateResourceRequestDto updateDto, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var lesson = await _resourceRepo.GetByIdAsync(guid, moduleId) ?? throw new KeyNotFoundException("Resource not found");
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this resource");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this resource");
        }
        var resourceUpdate = await _resourceRepo.UpdateAsync(guid, updateDto, moduleId);
        return OkResponse("lesson updated successfully", resourceUpdate!.ToResourceDto());
    }
}
