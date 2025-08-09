using System.Security.Claims;
using Khourse.Api.Services.IServices;

namespace Khourse.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId { get; }
    public string? Email { get; }
    public string? Role { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user != null)
        {
            UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            Email = user.FindFirstValue(ClaimTypes.Email);
            Role = user.FindFirstValue(ClaimTypes.Role);
        }
    }
}
