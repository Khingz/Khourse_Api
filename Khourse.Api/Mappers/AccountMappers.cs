using System;
using Khourse.Api.Dtos.Account;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class AccountMappers
{
    public static AuthUserDto ToAuthUserDto(this AppUser user, string token)
    {
        return new AuthUserDto
        {
            AccessToken = token,
            User = new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
            }
        };
    }
}
