namespace DTOs.Users;

public class CompatibilityResponse
{
    public float Compatibility { get; set; }
    public List<string> TopArtists { get; set; } = [];
}