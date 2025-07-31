using Khourse.Api.Dtos.Account;
using Khourse.Api.Exceptions;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;

namespace Khourse.Api.Services;

public class AccountService(IAccountRepository accountRepo, ITokenService tokenService) : IAccountService
{
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthUserDto> RegisterAccount(RegisterDto userDto)
    {
        var user = new AppUser
        {
            Email = userDto.Email,
            UserName = userDto.Email,
            Firstname = userDto.FirstName,
            Lastname = userDto.LastName
        };

        var result = await _accountRepo.CreateUserAsync(user, userDto.Password!);
        if (!result.Succeeded)
            throw new IdentityErrorException(result.Errors);

        var roleResult = await _accountRepo.AddUserToRoleAsync(user, "Student");
        if (!roleResult.Succeeded)
            throw new IdentityErrorException(roleResult.Errors);

        var userRole = await _accountRepo.GetUserRolesAsync(user);
        var token = await _tokenService.CreateToken(user);

        var newUser = user.ToAuthUserDto(token, userRole);

        return newUser;
    }

    public async Task<AuthUserDto> LoginAccount(LoginDto loginDto)
    {
        var user = await _accountRepo.LoginUserAsync(loginDto);
        var userRole = await _accountRepo.GetUserRolesAsync(user);
        var token = await _tokenService.CreateToken(user);

        var loggedInUser = user.ToAuthUserDto(token, userRole);
        return loggedInUser;
    }

    public async Task<bool> UpdateUserRole(UpdateRoleDto roleDto, string userId)
    {
        if (roleDto.NewRole!.Equals(roleDto.OldRole, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new BadHttpRequestException("You are already a student");
        }
        await _accountRepo.UpdateRoleAsync(userId, roleDto);
        return true;
    }

    public async Task<UserDto> GetUserById(string userId)
    {
        var user = await _accountRepo.UserByIdAsync(userId) ?? throw new KeyNotFoundException("User not found!");
        var userRole = await _accountRepo.GetUserRolesAsync(user);
        return user.ToUserDto(userRole);
    }
}
