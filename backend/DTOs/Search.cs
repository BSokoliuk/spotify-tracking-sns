using Models;

namespace DTOs;

public class SearchResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<UserProfile> Users { get; set; } = [];
    public List<Song> Songs { get; set; } = [];
    public List<Album> Albums { get; set; } = [];
    public List<Artist> Artists { get; set; } = [];
}

public class UserProfile
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = [];
}