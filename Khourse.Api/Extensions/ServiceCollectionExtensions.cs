using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomValidationResponses(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(entry => entry.Value != null && entry.Value.Errors.Count > 0)
                    .Select(entry => new
                    {
                        Field = entry.Key,
                        Errors = entry.Value!.Errors.Select(e => e.ErrorMessage)
                    });

                return new UnprocessableEntityObjectResult(new
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            };
        });

        return services;
    }
}
