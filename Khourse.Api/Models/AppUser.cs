using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Models;

public class AppUser : IdentityUser
{
    [Column("first_name")]
    public string? Firstname { get; set; }
    [Column("last_name")]
    public string? Lastname { get; set; }

}
