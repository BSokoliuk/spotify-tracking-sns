using System.ComponentModel.DataAnnotations;

namespace DTOs.Profile;

public class ChangeAvatarAdminRequest
{
    [Required]
    public string Avatar { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
}