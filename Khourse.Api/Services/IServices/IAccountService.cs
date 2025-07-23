using System;
using Khourse.Api.Dtos.Account;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Services.IServices;

public interface IAccountService
{
    Task<(bool Success, AuthUserDto User, IEnumerable<IdentityError> Errors)> RegisterAccount(RegisterDto userDto);

}
