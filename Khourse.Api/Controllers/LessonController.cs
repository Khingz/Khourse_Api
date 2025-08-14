using Khourse.Api.Common;
using Khourse.Api.Dtos.LessonDtos;
using Khourse.Api.Filters;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/modules/{moduleId}/lessons")]
[ServiceFilter(typeof(ModuleExistFilter))]
public class LessonController(ICurrentUserService currentUserService, IAccountRepository accountRepo, ILessonRepository lessonRepo) : BaseController
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly ILessonRepository _lessonRepo = lessonRepo;


    [Authorize(Roles = "Author,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateLesson(Guid moduleId, [FromBody] CreateLessonDto lessonDto)
    {
        var module = HttpContext.Items["Module"] as Module;
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to add module to this course");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to add lesson to this module");
        }
        var lessonModel = lessonDto.ToLessonEntity(module!.Id);
        await _lessonRepo.CreateAsync(lessonModel);
        var response = ApiSuccessResponse<LessonDto>.Ok(lessonModel.ToLessonDto(), "Lesson created successfully");
        return CreatedAtAction(nameof(GetById), new { moduleId = moduleId, id = lessonModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid lessonGuid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var lesson = await _lessonRepo.GetByIdAsync(lessonGuid, moduleId);
        return OkResponse("Module feteched successfully", lesson!.ToLessonDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllModuleLessons(Guid moduleId)
    {
        var lesson = await _lessonRepo.GetAllAsync(moduleId);
        var lessonDto = lesson.Select(l => l.ToLessonDto());
        return OkResponse("Lessons fetched successfully", lessonDto);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var lesson = await _lessonRepo.GetByIdAsync(guid, moduleId) ?? throw new KeyNotFoundException("Lesson not found");
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this Lesson");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this lesson");
        }
        await _lessonRepo.DeleteAsync(guid, moduleId);
        return OkResponse("Lesson deleted successfully", lesson.ToLessonDto());
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateLessonRequestDto updateDto, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var lesson = await _lessonRepo.GetByIdAsync(guid, moduleId) ?? throw new KeyNotFoundException("Lesson not found");
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this Lesson");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this lesson");
        }
        var lessonUpdate = await _lessonRepo.UpdateAsync(guid, updateDto, moduleId);
        return OkResponse("lesson updated successfully", lessonUpdate!.ToLessonDto());
    }
}
