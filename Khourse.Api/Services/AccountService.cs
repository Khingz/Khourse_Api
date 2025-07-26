using System;
using Khourse.Api.Dtos.Account;
using Khourse.Api.Exceptions;
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

        var token = await _tokenService.CreateToken(user);

        var newUser = user.ToAuthUserDto(token);

        return newUser;
    }

    public async Task<AuthUserDto> LoginAccount(LoginDto loginDto)
    {
        var user = await _accountRepo.LoginUserAsync(loginDto);
        var token = await _tokenService.CreateToken(user);

        var loggedInUser = user.ToAuthUserDto(token);
        return loggedInUser;
    }
}
