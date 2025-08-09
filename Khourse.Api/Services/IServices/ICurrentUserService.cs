namespace Khourse.Api.Services.IServices;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
}
