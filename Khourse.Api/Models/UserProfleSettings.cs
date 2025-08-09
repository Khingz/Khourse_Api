using System.ComponentModel.DataAnnotations.Schema;

namespace Khourse.Api.Models;

[Table("user_profile_settings")]
public class UserProfleSettings : BaseModel
{
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
}
