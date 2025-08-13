using System;
using Khourse.Api.Common;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Khourse.Api.Filters;

public class ModuleExistFilter(IModuleRepository moduleRepo) : IAsyncActionFilter
{
    private readonly IModuleRepository _moduleRepo = moduleRepo;
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
        var module = await _moduleRepo.ModueByIdAsync(moduleId);
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

        // Store course for controller access
        context.HttpContext.Items["Module"] = module;
        await next();
    }
}
