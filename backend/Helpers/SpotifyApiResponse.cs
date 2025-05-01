using Newtonsoft.Json;

namespace Helpers;

public class RecentlyPlayedResponse
{
    [JsonProperty("items")]
    public List<RecentlyPlayedItem> Items { get; set; } = [];

    [JsonProperty("next")]
    public string? Next { get; set; }

    [JsonProperty("cursors")]
    public Cursors Cursors { get; set; } = new();
}

public class RecentlyPlayedItem
{
    [JsonProperty("track")]
    public Track Track { get; set; } = new();

    [JsonProperty("played_at")]
    public DateTime PlayedAt { get; set; }
}

public class Track
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("album")]
    public SpotifyAlbum Album { get; set; } = new();

    [JsonProperty("artists")]
    public List<SpotifyArtist> Artists { get; set; } = [];
}

public class SpotifyAlbum
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
}

public class SpotifyArtist
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
}

public class Cursors
{
    [JsonProperty("after")]
    public string After { get; set; } = string.Empty;
}