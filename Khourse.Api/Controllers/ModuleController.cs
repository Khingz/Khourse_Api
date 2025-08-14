using Khourse.Api.Common;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Filters;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/courses/{courseId}/modules")]
[ServiceFilter(typeof(CourseExistFilter))]
public class ModuleController(IModuleRepository moduleRepo, ICurrentUserService currentUserService, IAccountRepository accountRepo) : BaseController
{
    private readonly IModuleRepository _moduleRepo = moduleRepo;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAccountRepository _accountRepo = accountRepo;


    [Authorize(Roles = "Author,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Guid courseId, [FromBody] CreateModuleDto module)
    {
        var course = HttpContext.Items["Course"] as Course;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to add module to this course");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != course!.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to add module to this course");
        }
        var moduleModel = module.ToModuleEntity(courseId);
        await _moduleRepo.CreateAsync(moduleModel);
        var response = ApiSuccessResponse<ModuleDto>.Ok(moduleModel.ToModuleDto(), "Module created successfully");
        return CreatedAtAction(nameof(GetById), new { courseId = courseId, id = moduleModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, Guid courseId)
    {
        if (!GuidUtils.TryParse(id, out Guid moduleGuid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var module = await _moduleRepo.GetByIdAsync(moduleGuid, courseId);
        return OkResponse("Module feteched successfully", module!.ToModuleDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourseModules(Guid courseId)
    {
        var modules = await _moduleRepo.GetAllAsync(courseId);
        var moduleDto = modules.Select(c => c.ToModuleDto());
        return OkResponse("Courses fetched successfully", moduleDto);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id, Guid courseId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var module = await _moduleRepo.GetByIdAsync(guid, courseId) ?? throw new KeyNotFoundException("Module not found");
        var course = HttpContext.Items["Course"] as Course;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this module");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != course!.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this module");
        }
        await _moduleRepo.DeleteAsync(guid, courseId);
        return OkResponse("Module deleted successfully", module.ToModuleDto());
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateModuleRequestDto updateDto, Guid courseId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var module = await _moduleRepo.GetByIdAsync(guid, courseId) ?? throw new KeyNotFoundException("Module not found");
        var course = HttpContext.Items["Course"] as Course;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this module");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != course!.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this module");
        }
        var moduleUpdate = await _moduleRepo.UpdateAsync(guid, updateDto, courseId);
        return OkResponse("Module updated successfully", moduleUpdate!.ToModuleDto());
    }
}
