using System.ComponentModel.DataAnnotations;

namespace DTOs.Profile;

public class ChangeAvatarRequest
{
    [Required]
    public string Avatar { get; set; } = string.Empty;
}