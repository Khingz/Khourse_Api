using System.ComponentModel.DataAnnotations;

namespace Khourse.Api.Dtos.Account;

public class RegisterDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please use a valid email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Firstname is required")]
    public string? Firstname { get; set; }

    [Required(ErrorMessage = "Lastname is required")]
    public string? Lastname { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
