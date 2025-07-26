using System.ComponentModel.DataAnnotations;

namespace Khourse.Api.Dtos.Account;

public class RegisterDto
{
    [Required(ErrorMessage = "email is required")]
    [EmailAddress(ErrorMessage = "Please use a valid email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "first_name is required")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "last_name is required")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "password is required")]
    public string? Password { get; set; }
}
