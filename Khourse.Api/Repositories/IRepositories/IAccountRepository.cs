using System;
using Khourse.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Repositories.IRepositories;

public interface IAccountRepository
{
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role);
}
