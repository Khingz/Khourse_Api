using System.ComponentModel.DataAnnotations;

public class UpdateRoleDto
{
    [Required(ErrorMessage = "new_role is required")]
    public string? NewRole { get; set; }
    [Required(ErrorMessage = "old_role is required")]
    public string? OldRole { get; set; }
}