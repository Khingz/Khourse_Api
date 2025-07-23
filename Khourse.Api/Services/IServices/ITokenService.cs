using System;
using Khourse.Api.Models;

namespace Khourse.Api.Services.IServices;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}