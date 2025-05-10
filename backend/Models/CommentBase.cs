namespace Models;

public abstract class CommentBase
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string SenderId { get; set; } = string.Empty;
    public virtual User Sender { get; set; } = null!;
}