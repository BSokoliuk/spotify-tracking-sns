using System.ComponentModel.DataAnnotations;

namespace DTOs.Profile;

public class ChangeBioAdminRequest
{
    [Required]
    public string EditedBio { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
}