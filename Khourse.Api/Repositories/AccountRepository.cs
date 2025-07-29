using Khourse.Api.Dtos.Account;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<AppUser?> UserByEmailAsync(string email)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            return null;
        }
        return user;

    }

    public async Task<AppUser?> UserByIdAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return null;
        }
        return user;
    }

    public async Task<AppUser> LoginUserAsync(LoginDto loginDto)
    {
        var user = await UserByEmailAsync(loginDto.Email!) ?? throw new UnauthorizedAccessException("Invalid email or password");
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        return user;
    }
}
