using Khourse.Api.Common;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Khourse.Api.Filters;

public class ModuleExistFilter(IModuleRepository moduleRepo, ICourseRepository courseRepo) : IAsyncActionFilter
{
    private readonly IModuleRepository _moduleRepo = moduleRepo;
    private readonly ICourseRepository _courseRepo = courseRepo;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.RouteData.Values.TryGetValue("moduleId", out var moduleIdValue)
            || !Guid.TryParse(moduleIdValue?.ToString(), out var moduleId))
        {
            context.Result = new BadRequestObjectResult(
                ApiErrorResponse.Fail(
                    StatusCodes.Status400BadRequest,
                    "Invalid or missing moduleId.",
                    [
                        new ErrorDetail { Code = "MODULE_ID_INVALID", Description = "The provided moduleId is not a valid GUID." }
                    ]
                )
            );
            return;
        }
        var module = await _moduleRepo.ModuleByIdAsync(moduleId);
        if (module == null)
        {
            context.Result = new NotFoundObjectResult(
                ApiErrorResponse.Fail(
                    StatusCodes.Status404NotFound,
                    $"Module with ID {moduleId} was not found.",

                    [
                        new ErrorDetail { Code = "MODULE_NOT_FOUND", Description = "No module exists with the specified ID." }
                    ]
                )
            );
            return;
        }

        var course = await _courseRepo.CourseById(module.CourseId!.Value);
        if (course == null)
        {
            context.Result = new NotFoundObjectResult(
                ApiErrorResponse.Fail(
                    StatusCodes.Status404NotFound,
                    $"Course not found",

                    [
                        new ErrorDetail { Code = "COURSE_NOT_FOUND", Description = "No course exists with the specified module ID." }
                    ]
                )
            );
            return;
        }

        // Store course for controller access
        context.HttpContext.Items["Module"] = module;
        context.HttpContext.Items["OwnerId"] = course.AuthorId;
        await next();
    }
}
