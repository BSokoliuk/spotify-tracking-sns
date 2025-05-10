using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("song_comments")]
public class SongComment : CommentBase
{
    public string SongId { get; set; } = string.Empty;
    public virtual Song Song { get; set; } = null!;
}