using System;
using Khourse.Api.Dtos.Account;
using Khourse.Api.Mappers;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Services;

public class AccountService(IAccountRepository accountRepo, ITokenService tokenService) : IAccountService
{
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<(bool Success, AuthUserDto User, IEnumerable<IdentityError> Errors)> RegisterAccount(RegisterDto userDto)
    {
        var user = new AppUser
        {
            Email = userDto.Email,
            UserName = userDto.Email,
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname
        };

        var result = await _accountRepo.CreateUserAsync(user, userDto.Password!);
        if (!result.Succeeded)
            return (false, null, result.Errors)!;

        var roleResult = await _accountRepo.AddUserToRoleAsync(user, "Student");
        if (!roleResult.Succeeded)
            return (false, null, roleResult.Errors)!;

        var token = await _tokenService.CreateToken(user);

        var newUser = user.ToAuthUserDto(token);

        return (true, newUser, null)!;
    }
}
