using Khourse.Api.Dtos.Account;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IList<string>> GetUserRolesAsync(AppUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> UpdateRoleAsync(string userId, UpdateRoleDto roleDto)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleDto.NewRole!);
        if (!roleExists)
            throw new BadHttpRequestException($"Role '{roleDto.NewRole}' does not exist.");
        var user = await UserByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
        var currentRoles = await GetUserRolesAsync(user);
        var targetRole = currentRoles.FirstOrDefault(r => r == roleDto.OldRole) ?? throw new BadHttpRequestException($"User is not a {roleDto.OldRole}");
        var removeRole = await _userManager.RemoveFromRoleAsync(user, targetRole) ?? throw new BadHttpRequestException("Failed to remove current role");
        var addRoleResult = await AddUserToRoleAsync(user, roleDto.NewRole!);
        if (!addRoleResult.Succeeded)
        {
            await AddUserToRoleAsync(user, targetRole);
            throw new BadHttpRequestException("Failed to assign new role; original role restored");
        }
        return true;
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
