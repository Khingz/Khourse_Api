using System.Security.Claims;
using System.Text;
using Khourse.Api.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Khourse.Api.Extensions;

public static class JwtAuthenticationExtension
{
    public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme =
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
                ),
                RoleClaimType = ClaimTypes.Role
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse(); // Prevent default response

                    var errorResponse = ApiErrorResponse.Fail(
                        StatusCodes.Status401Unauthorized,
                        "Unauthorized",
                        [new ErrorDetail { Code = "Unauthorized", Description = "Authentication is required." }]
                    );

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var json = JsonConvert.SerializeObject(errorResponse, JsonConfig.SnakeCaseSettings);
                    return context.Response.WriteAsync(json);
                },
                OnForbidden = context =>
                {
                    var errorResponse = ApiErrorResponse.Fail(
                        StatusCodes.Status403Forbidden,
                        "Forbidden",
                        [new ErrorDetail { Code = "Forbidden", Description = "You do not have permission to access this resource." }]
                    );

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var json = JsonConvert.SerializeObject(errorResponse, JsonConfig.SnakeCaseSettings);
                    return context.Response.WriteAsync(json);
                }
            };
        });

        return services;
    }

}
