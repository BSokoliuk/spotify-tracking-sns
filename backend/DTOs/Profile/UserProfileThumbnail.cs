namespace DTOs.Profile;

public class CompactUserProfileDTO
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = [];
}