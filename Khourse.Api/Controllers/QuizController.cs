using System;
using Khourse.Api.Common;
using Khourse.Api.Dtos.QuizDtos;
using Khourse.Api.Filters;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/modules/{moduleId}/quizzes")]
[ServiceFilter(typeof(ModuleExistFilter))]
public class QuizController(ICurrentUserService currentUserService, IAccountRepository accountRepo, IQuizRepository quizRepo) : BaseController
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly IQuizRepository _quizRepo = quizRepo;

    [Authorize(Roles = "Author,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateQuiz(Guid moduleId, [FromBody] CreateQuizDto quizDto)
    {
        var module = HttpContext.Items["Module"] as Module;
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to add quiz to this module");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to add quiz to this module");
        }
        var quizModel = quizDto.ToQuizEntity(module!.Id);
        await _quizRepo.CreateAsync(quizModel);
        var response = ApiSuccessResponse<QuizDto>.Ok(quizModel.ToQuizDto(), "Quiz created successfully");
        return CreatedAtAction(nameof(GetById), new { moduleId = moduleId, id = quizModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid quizGuid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var quiz = await _quizRepo.GetByIdAsync(quizGuid, moduleId);
        return OkResponse("quiz feteched successfully", quiz!.ToQuizDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllModuleQuizzes(Guid moduleId)
    {
        var quiz = await _quizRepo.GetAllAsync(moduleId);
        var quizDto = quiz.Select(q => q.ToQuizDto());
        return OkResponse("Quiz fetched successfully", quizDto);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var quiz = await _quizRepo.GetByIdAsync(guid, moduleId) ?? throw new KeyNotFoundException("Quiz not found");
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to delete this quiz");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this quiz");
        }
        await _quizRepo.DeleteAsync(guid, moduleId);
        return OkResponse("Resource deleted successfully", quiz.ToQuizDto());
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] QuizUpdateRequestDto updateDto, Guid moduleId)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var quiz = await _quizRepo.GetByIdAsync(guid, moduleId) ?? throw new KeyNotFoundException("Quiz not found");
        string ownerId = (string)HttpContext.Items["OwnerId"]!;
        var currentUser = await _accountRepo.UserByIdAsync(_currentUserService.UserId!) ?? throw new UnauthorizedAccessException("You are not authorized to update this quiz");
        var isAdmin = await _accountRepo.UserHasRoleAsync(currentUser, "Admin");
        if (!isAdmin && _currentUserService.UserId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this quiz");
        }
        var quizUpdate = await _quizRepo.UpdateAsync(guid, updateDto, moduleId);
        return OkResponse("Quiz updated successfully", quizUpdate!.ToQuizDto());
    }

}
