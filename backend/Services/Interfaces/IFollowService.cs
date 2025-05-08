using Results;

namespace Services.Interfaces;

public interface IFollowService
{
    Task<CustomResult<bool>> FollowUser(string sourceUserId, string targetUserId);
    Task<CustomResult<bool>> UnfollowUser(string sourceUserId, string targetUserId);
    Task<CustomResult<bool>> IsFollowing(string sourceUserId, string targetUserId);
}