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
                    message = "Method Not Allowed",
                    error = new
                    {
                        code = 405,
                        description = "The requested endpoint does not exist.",
                    }
                };

                await context.HttpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        });
    }
}
