using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DTOs;
using Helpers;
using Services.Interfaces;
using Results;
using Models;

namespace Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;

    [HttpPost("{subject}/{subjectId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateComment(string subject, string subjectId, [FromBody] string content)
    {
        var userId = User.GetUserId();

        var result = await CreateCommentBySubject(subject, subjectId, content, userId);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new { message = result.Error.Message });

            return BadRequest(new { message = result.Error.Message });
        }

        return Ok(new { comment = result.Value });
    }

    [HttpDelete("{subject}/{commentId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteComment(string subject, string commentId)
    {
        var roles = User.GetRoles();
        var userId = User.GetUserId();

        var result = await DeleteCommentBySubject(subject, commentId, roles, userId);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new { message = result.Error.Message });

            return BadRequest(new { message = result.Error.Message });
        }

        return Ok(new ApiResponse(true, "Comment deleted successfully"));
    }

    [HttpPatch("{subject}/{commentId}")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditComment(string subject, string commentId, [FromBody] string newContent)
    {
        var result = await EditCommentBySubject(subject, commentId, newContent);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new { message = result.Error.Message });

            return BadRequest(new { message = result.Error.Message });
        }

        return Ok(new ApiResponse(result.Value, "Comment edited successfully"));
    }

    private async Task<CustomResult<object>> CreateCommentBySubject(string subject, string subjectId, string content, string userId)
    {
        return subject.ToLower() switch
        {
            "profile" => (await _commentService.CreateComment<ProfileComment>(subjectId, content, userId)).Cast<object>(),
            "song" => (await _commentService.CreateComment<SongComment>(subjectId, content, userId)).Cast<object>(),
            "album" => (await _commentService.CreateComment<AlbumComment>(subjectId, content, userId)).Cast<object>(),
            "artist" => (await _commentService.CreateComment<ArtistComment>(subjectId, content, userId)).Cast<object>(),
            _ => CustomResult<object>.Failure(CustomError.ValidationError("Invalid subject type"))
        };
    }

    private async Task<CustomResult<bool>> DeleteCommentBySubject(string subject, string commentId, List<string> roles, string userId)
    {
        return subject.ToLower() switch
        {
            "profile" => await _commentService.DeleteComment<ProfileComment>(commentId, roles, userId),
            "song" => await _commentService.DeleteComment<SongComment>(commentId, roles, userId),
            "album" => await _commentService.DeleteComment<AlbumComment>(commentId, roles, userId),
            "artist" => await _commentService.DeleteComment<ArtistComment>(commentId, roles, userId),
            _ => CustomResult<bool>.Failure(CustomError.ValidationError("Invalid subject type"))
        };
    }

    private async Task<CustomResult<bool>> EditCommentBySubject(string subject, string commentId, string newContent)
    {
        return subject.ToLower() switch
        {
            "profile" => await _commentService.EditComment<ProfileComment>(commentId, newContent),
            "song" => await _commentService.EditComment<SongComment>(commentId, newContent),
            "album" => await _commentService.EditComment<AlbumComment>(commentId, newContent),
            "artist" => await _commentService.EditComment<ArtistComment>(commentId, newContent),
            _ => CustomResult<bool>.Failure(CustomError.ValidationError("Invalid subject type"))
        };
    }
}