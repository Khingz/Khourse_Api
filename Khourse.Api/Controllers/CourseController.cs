using System;
using Khourse.Api.Common;
using Khourse.Api.Data;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Mappers;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/courses")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _courseRepo;
    public CourseController(ICourseRepository courseRepo)
    {
        _courseRepo = courseRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseRepo.GetAllAsync();
        var courseDto = courses.Select(c => c.ToCourseDto());
        return Ok(ApiResponse<object>.Success(courseDto, "Courses retrieved."));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequestDto courseDto)
    {
        var courseModel = courseDto.ToCourseEntity();
        await _courseRepo.CreateAsync(courseModel);
        return CreatedAtAction(nameof(GetById), new { id = courseModel.Id }, courseModel.ToCourseDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var course = await _courseRepo.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<string>.Fail("Course not found."));
        }
        return Ok(ApiResponse<object>.Success(course.ToCourseDto(), "Course retrieved."));

    }
}
