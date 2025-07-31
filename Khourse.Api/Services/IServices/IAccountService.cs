using Khourse.Api.Dtos.Account;

namespace Khourse.Api.Services.IServices;

public interface IAccountService
{
    Task<AuthUserDto> RegisterAccount(RegisterDto userDto);
    Task<AuthUserDto> LoginAccount(LoginDto loginDto);
    Task<bool> UpdateUserRole(UpdateRoleDto roleDto, string userId);
    Task<UserDto> GetUserById(string userId);
}
