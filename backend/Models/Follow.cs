using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("follows")]
public class Follow
{
  public string Id { get; set; } = string.Empty;

  public string FollowerId { get; set; } = string.Empty; // ID of the user who is following
  public string FollowedId { get; set; } = string.Empty; // ID of the user being followed
  
  // navigation properties
  public virtual User Follower { get; set; } = null!;
  public virtual User Followed { get; set; } = null!;
}