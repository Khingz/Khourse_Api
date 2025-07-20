using Khourse.Api.Common;
using Khourse.Api.Dtos.ModuleDtos;
using Khourse.Api.Mappers;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/modules")]
public class ModuleController(IModuleRepository moduleRepo, ICourseRepository courseRepo) : BaseController
{
    private readonly IModuleRepository _moduleRepo = moduleRepo;
    private readonly ICourseRepository _courseRepo = courseRepo;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateModuleDto module)
    {
        Guid courseId = module.CourseId!.Value;
        if (!await _courseRepo.CourseExists(courseId))
        {
            return BadRequestResponse("Course does not exist");
        }
        var moduleModel = module.ToModuleEntity();
        await _moduleRepo.CreateAsync(moduleModel);
        var response = ApiResponse<ModuleDto>.Ok("Module created successfully", moduleModel.ToModuleDto());
        return CreatedAtAction(nameof(GetById), new { id = moduleModel.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            return BadRequestResponse("Invalid GUID format.");
        }
        var course = await _moduleRepo.GetByIdAsync(guid);
        if (course == null)
        {
            return ErrorResponse(404, "Not Found", "Module not found");
        }
        return OkResponse("Module feteched successfully", course);

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var modules = await _moduleRepo.GetAllAsync();
        var moduleDto = modules.Select(c => c.ToModuleDto());
        return OkResponse("Courses fetched successfully", moduleDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            return BadRequestResponse("Invalid GUID format.");
        }
        var module = await _moduleRepo.DeleteAsync(guid);
        if (module == null)
        {
            return ErrorResponse(404, "Not Found", "Module not found");
        }
        return OkResponse("module deleted successfully", module);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateModuleRequestDto updateDto)
    {
        if (!GuidUtils.TryParse(id, out Guid guid))
        {
            return BadRequestResponse("Invalid GUID format.");
        }
        var module = await _moduleRepo.UpdateAsync(guid, updateDto);
        if (module == null)
        {
            return ErrorResponse(404, "Not Found", "Module not found");
        }
        return OkResponse("Module feteched successfully", module);
    }
}
