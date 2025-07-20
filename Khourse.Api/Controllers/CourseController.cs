using Khourse.Api.Common;
using Khourse.Api.Dtos.CourseDtos;
using Khourse.Api.Mappers;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/courses")]
public class CourseController : BaseController
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
        return OkResponse("Courses fetched successfully", courseDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequestDto courseDto)
    {
        var courseModel = courseDto.ToCourseEntity();
        await _courseRepo.CreateAsync(courseModel);
        var response = ApiResponse<CourseDto>.Ok("Course created successfully", courseModel.ToCourseDto());
        return CreatedAtAction(nameof(GetById), new { id = courseModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            return BadRequestResponse("Invalid GUID format.");
        }
        var course = await _courseRepo.GetByIdAsync(guid);
        if (course == null)
        {
            return ErrorResponse(404, "Not Found", "Course not found");
        }
        return OkResponse("Course feteched successfully", course);

    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCourseRequestDto updateDto)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            return BadRequestResponse("Invalid GUID format.");
        }
        var course = await _courseRepo.UpdateAsync(guid, updateDto);
        if (course == null)
        {
            return ErrorResponse(404, "Not Found", "Course not found");
        }
        return OkResponse("Course feteched successfully", course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            return BadRequestResponse("Invalid GUID format.");
        }
        var course = await _courseRepo.DeleteAsync(guid);
        if (course == null)
        {
            return ErrorResponse(404, "Not Found", "Course not found");
        }
        return OkResponse("Course deleted successfully", course);
    }
}
