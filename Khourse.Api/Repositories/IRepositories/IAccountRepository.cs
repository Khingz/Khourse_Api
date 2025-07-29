using Khourse.Api.Dtos.Account;
using Khourse.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Repositories.IRepositories;

public interface IAccountRepository
{
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role);
    Task<AppUser> LoginUserAsync(LoginDto loginDto);
    Task<AppUser?> UserByIdAsync(string id);
    Task<AppUser?> UserByEmailAsync(string email);
}
