using Khourse.Api.Common;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Helpers;
using Khourse.Api.Mappers;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/courses")]
public class CourseController(ICourseRepository courseRepo, ICurrentUserService currentUserService) : BaseController
{
    private readonly ICourseRepository _courseRepo = courseRepo;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CourseQueryOject query)
    {
        var courses = await _courseRepo.GetAllAsync(query);
        return OkResponse("Courses fetched successfully", courses);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequestDto courseDto)
    {
        Console.WriteLine(_currentUserService.Role);
        var courseModel = courseDto.ToCourseEntity(_currentUserService.UserId!);
        var result = await _courseRepo.CreateAsync(courseModel);
        var response = ApiSuccessResponse<CourseDto>.Ok(result, "Course created successfully");
        return CreatedAtAction(nameof(GetById), new { id = courseModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");
        }
        var course = await _courseRepo.GetByIdAsync(guid);
        return OkResponse("Course feteched successfully", course);

    }

    [Authorize(Roles = "Admin,Author")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCourseRequestDto updateDto)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");
        }
        var course = await _courseRepo.UpdateAsync(guid, updateDto, _currentUserService.UserId!);
        return OkResponse("Course feteched successfully", course);
    }

    [Authorize(Roles = "Author,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");
        }
        var course = await _courseRepo.DeleteAsync(guid, _currentUserService.UserId!);
        return OkResponse("Course deleted successfully", course);
    }

    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetCourseModules([FromRoute] string id, [FromQuery] QueryObject query)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");

        }
        var moduleData = await _courseRepo.GetCourseModulesAsync(guid, query);
        return OkResponse("Courses fetched successfully", moduleData);
    }
}
