using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("profileComments")]
public class ProfileComment : CommentBase
{
  public string RecipientId { get; set; } = string.Empty;
  public virtual User Recipient { get; set; } = null!;
}