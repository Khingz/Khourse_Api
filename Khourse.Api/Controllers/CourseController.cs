using System;
using Khourse.Api.Data;
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
    public async Task<IActionResult> GetAllAsync()
    {
        var courses = await _courseRepo.GetAllAsync();
        return Ok(courses);
    }
}
