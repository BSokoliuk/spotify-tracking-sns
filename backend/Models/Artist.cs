using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("artists")]
public class Artist
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Id_Artist_Spotify_API { get; set; } = string.Empty;
    public byte[] Photo { get; set; } = [];
    public string Description { get; set; } = string.Empty;

    public ICollection<Album> Albums { get; set; } = [];
    public ICollection<ArtistComment> ArtistComments { get; set; } = [];
    public ICollection<ArtistRating> ArtistRatings { get; set; } = [];
}

[Table("artistsRating")]
public class ArtistRating
{
    public string Id { get; set; } = string.Empty;
    public int Rating { get; set; } = 0;

    public string Id_User { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public string Id_Artist_Internal { get; set; } = string.Empty;
    public Artist Artist { get; set; } = null!;
}