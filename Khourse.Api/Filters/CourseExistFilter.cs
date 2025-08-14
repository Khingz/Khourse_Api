using Khourse.Api.Common;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Khourse.Api.Filters;

public class CourseExistFilter(ICourseRepository courseRepo) : IAsyncActionFilter
{

    private readonly ICourseRepository _courseRepo = courseRepo;
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.RouteData.Values.TryGetValue("courseId", out var courseIdValue)
            || !Guid.TryParse(courseIdValue?.ToString(), out var courseId))
        {
            context.Result = new BadRequestObjectResult(
                ApiErrorResponse.Fail(
                    StatusCodes.Status400BadRequest,
                    "Invalid or missing courseId.",
                    [
                        new ErrorDetail { Code = "COURSE_ID_INVALID", Description = "The provided courseId is not a valid GUID." }
                    ]
                )
            );
            return;
        }
        var course = await _courseRepo.CourseById(courseId);
        if (course == null)
        {
            context.Result = new NotFoundObjectResult(
                ApiErrorResponse.Fail(
                    StatusCodes.Status404NotFound,
                    $"Course with ID {courseId} was not found.",

                    [
                        new ErrorDetail { Code = "COURSE_NOT_FOUND", Description = "No course exists with the specified ID." }
                    ]
                )
            );
            return;
        }

        // Store course for controller access
        context.HttpContext.Items["Course"] = course;
        await next();
    }
}
