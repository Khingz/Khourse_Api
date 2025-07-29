using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Khourse.Api.Models;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Khourse.Api.Services;

public class TokenService : ITokenService
{

    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<AppUser> _userManager;

    // Notice we use primary consructor as opposed to regular constructor, 
    // Field initializers run before the constructor, but _config is injected during construction. 
    // So _config doesn’t exist yet when _key is being initialized outside a constructor hence a primary sonstuctor is defined.
    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        _userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser user)
    {
        // Get user role to add to claim ==> returns a list of roles and all roles are added to claim
        var roles = await _userManager.GetRolesAsync(user);


        // create claim to be added to the token ==> this adds email, id and user role
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // sign credentials
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        // Define token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(double.Parse(_config["Jwt:ExpiryDate"]!)),
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };


        //  initializing the default handler for handling token related operations (create,validate etc)
        var tokenHandler = new JwtSecurityTokenHandler();

        // create token using token handler
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // convert token using token handler and return token
        return tokenHandler.WriteToken(token);

    }
}
