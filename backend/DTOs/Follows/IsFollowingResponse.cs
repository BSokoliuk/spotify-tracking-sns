namespace DTOs.Follows;

public class IsFollowingResponse
{
    public bool IsFollowing { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
}