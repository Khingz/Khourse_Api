using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Khourse.Api.Extensions;

public static class MethodNotFoundMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMethodNotFoundHandler(this IApplicationBuilder app)
    {
        return app.UseStatusCodePages(async context =>
        {
            if (context.HttpContext.Response.StatusCode == 405)
            {
                context.HttpContext.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    success = false,
                    error = new
                    {
                        code = 405,
                        type = "MethodNotFound",
                        message = "The requested endpoint does not exist."
                    }
                };

                await context.HttpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        });
    }
}
