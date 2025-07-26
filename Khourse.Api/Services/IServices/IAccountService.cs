using System;
using Khourse.Api.Dtos.Account;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Services.IServices;

public interface IAccountService
{
    Task<AuthUserDto> RegisterAccount(RegisterDto userDto);
    Task<AuthUserDto> LoginAccount(LoginDto loginDto);

}
