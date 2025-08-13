using Khourse.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/courses/{courseId}/modules")]
[ServiceFilter(typeof(ModuleExistFilter))]
public class LessonController : BaseController
{
    // public Task<IActionResult> CreateLesson()
    // {
    //     return OkResponse("Lesson created successfuly");
    // }
}
