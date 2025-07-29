namespace Khourse.Api.Dtos.Account;

public class AuthUserDto
{
    public string? AccessToken { get; set; }
    public UserDto? User { get; set; }
}
