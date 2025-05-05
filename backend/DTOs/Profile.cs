using System.ComponentModel.DataAnnotations;
using Models;

namespace DTOs;

public class ProfileResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = [];
    public string Description { get; set; } = string.Empty;
    public int ArtistCount { get; set; } = 0;
    public DateTime Creation_Date { get; set; } = DateTime.Now;
    public List<Follows> Followers { get; set; } = [];
    public List<Follows> Following { get; set; } = [];
    public List<ProfileComments> ProfileComments { get; set; } = [];
    public List<Scrobbles> Scrobbles { get; set; } = [];
    public int ScrobblesCount { get; set; } = 0;
    public List<RatedSongs> RatedSongs { get; set; } = [];
    public List<RatedAlbums> RatedAlbums { get; set; } = [];
    public List<RatedArtists> RatedArtists { get; set; } = [];
    public List<FavouriteSongs> FavouriteSongs { get; set; } = [];
    public byte[] TopArtistImage { get; set; } = [];
    public string RefreshToken { get; set; } = string.Empty;
}

public class Follows
{
    public string Id { get; set; } = string.Empty;
    public string Id_Follower { get; set; } = string.Empty;
    public string Id_Followed { get; set; } = string.Empty;
    public FollowerData Follower { get; set; } = null!;
    public FollowerData Followed { get; set; } = null!;
}

public class ProfileComments
{
    public string Id { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime Creation_Date { get; set; } = DateTime.Now;

    public string Id_Sender { get; set; } = string.Empty;

    public string Id_Recipient { get; set; } = string.Empty;
    public Sender Sender { get; set; } = null!;
}

public class Scrobbles
{
    public string Id { get; set; } = string.Empty;
    public DateTime Scrobble_Date { get; set; }

    public string Id_User { get; set; } = string.Empty;
    public string Id_Song_Internal { get; set; } = string.Empty;
    public Song Song { get; set; } = null!;
    public double AvgRating { get; set; } = 0;
}

public class RatedSongs
{
    public string Id_Song { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Id_Song_Internal { get; set; } = string.Empty;
    public Song Song { get; set; } = null!;
}

public class RatedAlbums
{
    public string Id_Album { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Id_Album_Internal { get; set; } = string.Empty;
    public Album Album { get; set; } = null!;
}

public class RatedArtists
{
    public string Id_Artist { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Id_Artist_Internal { get; set; } = string.Empty;
    public Artist Artist { get; set; } = null!;
}

public class FavouriteSongs
{
    public string Id_Song { get; set; } = string.Empty;
    public string Id_Song_Internal { get; set; } = string.Empty;
    public Song Song { get; set; } = null!;
}

public class EditUsersProfileRequest
{
    public string Bio { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;
}

public class Sender
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = [];
}

public class FollowerData
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = [];
}

public class ConnectSpotifyRequest
{
    public string RefreshToken { get; set; } = string.Empty;
    public string Id_User_Spotify_API { get; set; } = string.Empty;
}

public class MostActiveUsers
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public byte[] ProfilePicture { get; set; } = [];
    public int ScrobbleCount { get; set; } = 0;
}

public class ChangeBioRequest
{
    [Required]
    public string EditedBio { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
}

public class ChangeAvatarRequest
{
    [Required]
    public string Avatar { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
}