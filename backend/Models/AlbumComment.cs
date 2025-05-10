using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("album_comments")]
public class AlbumComment : CommentBase
{
    public string AlbumId { get; set; } = string.Empty;
    public virtual Album Album { get; set; } = null!;
}