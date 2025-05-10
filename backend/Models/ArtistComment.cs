using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("artist_comments")]
public class ArtistComment : CommentBase
{
    public string ArtistId { get; set; } = string.Empty;
    public virtual Artist Artist { get; set; } = null!;
}