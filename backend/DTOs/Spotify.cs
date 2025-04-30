namespace DTOs;

public class SongRecommendations
{
    public List<RecommendedSong> Songs { get; set; } = [];
}

public class RecommendedSong
{
    public required string Title { get; set; }
    public required string Id { get; set; }
    public required string Artist { get; set; }
    public required string Cover { get; set; }
}

public class ArtistRecommendations
{
    public List<RecommendedArtist> Artists { get; set; } = [];
}

public class RecommendedArtist
{
    public required string Name { get; set; }
    public required string Id { get; set; }
    public required string Photo { get; set; }
}