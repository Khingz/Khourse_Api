using System;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }
}
