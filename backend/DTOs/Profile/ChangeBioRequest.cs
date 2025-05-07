using System.ComponentModel.DataAnnotations;

namespace DTOs.Profile;

public class ChangeBioRequest
{
    [Required]
    public string EditedBio { get; set; } = string.Empty;
}