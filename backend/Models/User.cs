using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Services;

namespace Models;

[Table("users")]
public class User : IdentityUser
{
  public string RefreshToken { get; internal set; } = string.Empty;
  public byte[] Avatar { get; internal set; } = AuthenticationService.getDefaultAvatar();
  public string Bio { get; internal set; } = string.Empty;
  public string Id_User_Spotify_API { get; internal set; } = string.Empty;
  public DateTime Creation_Date { get; internal set; } = DateTime.Now.ToUniversalTime();

  public ICollection<Follow> Follows { get; set; } = [];
  public ICollection<Follow> Followers { get; set; } = [];
  public ICollection<ProfileComment> ProfileComments { get; set; } = [];
  public ICollection<FavouriteSong> FavouriteSongs { get; set; } = [];
  public ICollection<SongComment> SongComments { get; set; } = [];
  public ICollection<SongRating> SongRatings { get; set; } = [];
  public ICollection<AlbumRating> AlbumRatings { get; set; } = [];
  public ICollection<AlbumComment> AlbumComments { get; set; } = [];
  public ICollection<ArtistRating> ArtistRatings { get; set; } = [];
  public ICollection<ArtistComment> ArtistComments { get; set; } = [];

  public ICollection<Scrobble> Scrobbles { get; set; } = [];
}

[Table("follows")]
public class Follow
{
  public string Id { get; set; } = string.Empty;

  public string Id_Follower { get; set; } = string.Empty;
  public User Follower { get; set; } = null!;

  public string Id_Followed { get; set; } = string.Empty;
  public User Followed { get; set; } = null!;
}

[Table("profileComments")]
public class ProfileComment
{
  public string Id { get; set; } = string.Empty;
  public string Comment { get; set; } = string.Empty;
  public DateTime Creation_Date { get; set; } = DateTime.Now;

  public string Id_Sender { get; set; } = string.Empty;
  public User Sender { get; set; } = null!;

  public string Id_Recipient { get; set; } = string.Empty;
}