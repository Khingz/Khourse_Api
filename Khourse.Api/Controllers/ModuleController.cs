using Khourse.Api.Common;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Mappers;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/modules")]
public class ModuleController(IModuleRepository moduleRepo, ICourseRepository courseRepo, ICurrentUserService currentUserService, IAccountRepository accountRepo) : BaseController
{
    private readonly IModuleRepository _moduleRepo = moduleRepo;
    private readonly ICourseRepository _courseRepo = courseRepo;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAccountRepository _accountRepo = accountRepo;


    [Authorize(Roles = "Author,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateModuleDto module)
    {
        Guid courseId = module.CourseId!.Value;
        var course = await _courseRepo.CourseById(courseId) ?? throw new KeyNotFoundException("Course does not exist");
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to add module to this course");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != course.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to add module to this course");
        }
        var moduleModel = module.ToModuleEntity();
        await _moduleRepo.CreateAsync(moduleModel);
        var response = ApiSuccessResponse<ModuleDto>.Ok(moduleModel.ToModuleDto(), "Module created successfully");
        return CreatedAtAction(nameof(GetById), new { id = moduleModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var module = await _moduleRepo.GetByIdAsync(guid);
        return OkResponse("Module feteched successfully", module!.ToModuleDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var modules = await _moduleRepo.GetAllAsync();
        var moduleDto = modules.Select(c => c.ToModuleDto());
        return OkResponse("Courses fetched successfully", moduleDto);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var module = await _moduleRepo.GetByIdAsync(guid) ?? throw new KeyNotFoundException("Module not found");
        Guid courseId = module.CourseId!.Value;
        var course = await _courseRepo.CourseById(courseId) ?? throw new KeyNotFoundException("Course does not exist");
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this module");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != course.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this module");
        }
        await _moduleRepo.DeleteAsync(guid);
        return OkResponse("Module deleted successfully", module.ToModuleDto());
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateModuleRequestDto updateDto)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var module = await _moduleRepo.GetByIdAsync(guid) ?? throw new KeyNotFoundException("Module not found");
        Guid courseId = module.CourseId!.Value;
        var course = await _courseRepo.CourseById(courseId) ?? throw new KeyNotFoundException("Course does not exist");
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this module");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != course.AuthorId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this module");
        }
        var moduleUpdate = await _moduleRepo.UpdateAsync(guid, updateDto);
        return OkResponse("Module updated successfully", moduleUpdate!.ToModuleDto());
    }
}
