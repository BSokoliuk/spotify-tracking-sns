using Data;
using Models;
using Results;
using Services.Interfaces;
using UoW;

namespace Services;

public class CommentService(IUnitOfWork unitOfWork) : ICommentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CustomResult<T>> CreateComment<T>(string subjectId, string content, string userId) where T : CommentBase, new()
    {
        // Validate comment content
        if (string.IsNullOrWhiteSpace(content) || content.Length > 500)
        {
            return CustomResult<T>.Failure(CustomError.ValidationError("Comment content is invalid"));
        }

        // Fetch user
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        if (user is null)
        {
            return CustomResult<T>.Failure(CustomError.RecordNotFound("User not found"));
        }

        // Check if the subject exists
        var subjectExists = typeof(T) switch
        {
            var type when type == typeof(ProfileComment) => await _unitOfWork.Repository<User>().AnyAsync(u => u.Id == subjectId),
            var type when type == typeof(SongComment) => await _unitOfWork.Repository<Song>().AnyAsync(s => s.Id == subjectId),
            var type when type == typeof(AlbumComment) => await _unitOfWork.Repository<Album>().AnyAsync(a => a.Id == subjectId),
            var type when type == typeof(ArtistComment) => await _unitOfWork.Repository<Artist>().AnyAsync(a => a.Id == subjectId),
            _ => false
        };

        if (!subjectExists)
        {
            return CustomResult<T>.Failure(CustomError.RecordNotFound("Subject not found"));
        }

        // Create the comment
        var comment = new T
        {
            Id = Guid.NewGuid().ToString(),
            Content = content,
            CreatedAt = DateTime.UtcNow,
            SenderId = user.Id,
            Sender = user
        };

        // Configure additional properties for specific comment types
        switch (comment)
        {
            case ProfileComment profileComment:
                profileComment.RecipientId = subjectId;
                break;
            case SongComment songComment:
                songComment.SongId = subjectId;
                break;
            case AlbumComment albumComment:
                albumComment.AlbumId = subjectId;
                break;
            case ArtistComment artistComment:
                artistComment.ArtistId = subjectId;
                break;
        }

        await _unitOfWork.Repository<T>().AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<T>.Success(comment);
    }

    public async Task<CustomResult<bool>> EditComment<T>(string id, string newContent) where T : CommentBase
    {
        var comment = await _unitOfWork.Repository<T>().GetByIdAsync(id);
        if (comment is null)
        {
            return CustomResult<bool>.Failure(CustomError.RecordNotFound("Comment not found"));
        }

        comment.Content = newContent;
        _unitOfWork.Repository<T>().Update(comment);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<bool>.Success(true);
    }

  public async Task<CustomResult<bool>> DeleteComment<T>(string id, List<string> roles, string userId) where T : CommentBase
    {
        // Fetch the comment
        var comment = await _unitOfWork.Repository<T>().GetByIdAsync(id);
        if (comment is null)
        {
            return CustomResult<bool>.Failure(CustomError.RecordNotFound("Comment not found"));
        }

        bool isAdmin = roles.Contains("Admin"); // has admin role
        bool isOwner = comment.SenderId == userId; // is the owner of the comment
        bool isRecipient = typeof(T) == typeof(ProfileComment) // is a profile comment
            && ((ProfileComment)(object)comment).RecipientId == userId; // is the recipient of the comment

        bool isDeletionAllowed = isAdmin || isOwner || isRecipient;

        if (isDeletionAllowed)
        {
            _unitOfWork.Repository<T>().Delete(comment);
            await _unitOfWork.SaveChangesAsync();
            return CustomResult<bool>.Success(true);
        }

        return CustomResult<bool>.Failure(CustomError.ValidationError("You do not have permission to delete this comment"));
    }
}