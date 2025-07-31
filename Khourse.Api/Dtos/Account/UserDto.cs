using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Dtos.Account;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }
    public IList<string> Roles { get; set; } = [];
}
