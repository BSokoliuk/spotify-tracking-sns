using DTOs.Profile;
using Models;

namespace DTOs.Search;

public class SearchResponse
{
    public bool Success { get; set; } = true;
    public List<CompactUserProfileDTO> Users { get; set; } = new();
    public List<Song> Songs { get; set; } = new();
    public List<Album> Albums { get; set; } = new();
    public List<Artist> Artists { get; set; } = new();
}