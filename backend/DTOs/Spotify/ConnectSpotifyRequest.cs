namespace DTOs.Spotify;

public class ConnectSpotifyRequest
{
    public string RefreshToken { get; set; } = string.Empty;
    public string Id_User_Spotify_API { get; set; } = string.Empty;
}
