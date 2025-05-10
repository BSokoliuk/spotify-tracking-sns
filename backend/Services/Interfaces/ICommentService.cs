using Models;
using Results;

namespace Services.Interfaces;

public interface ICommentService
{
    Task<CustomResult<T>> CreateComment<T>(string subjectId, string content, string userId) where T : CommentBase, new();
    Task<CustomResult<bool>> DeleteComment<T>(string commentId, List<string> roles, string userId) where T : CommentBase;
    Task<CustomResult<bool>> EditComment<T>(string commentId, string newContent) where T : CommentBase;
}