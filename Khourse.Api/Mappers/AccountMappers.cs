using Khourse.Api.Dtos.Account;
using Khourse.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Mappers;

public static class AccountMappers
{
    public static AuthUserDto ToAuthUserDto(this AppUser user, string token, IList<string> role)
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
                Roles = role
            }
        };
    }

    public static UserDto ToUserDto(this AppUser user, IList<string> role)
    {
        return new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Roles = role
        };
    }
}
