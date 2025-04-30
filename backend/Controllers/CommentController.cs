using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DTOs;
using Helpers;

namespace Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;
    private readonly AuthenticationService _authenticationService;

    public CommentController(CommentService commentService, AuthenticationService authenticationService)
    {
        _commentService = commentService;
        _authenticationService = authenticationService;
    }


    [HttpGet("profile/{id}")]
    public async Task<IActionResult> ProfileCommentById(string id)
    {
        try
        {
            var comment = await _commentService.GetProfileCommentById(id);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }

    }

    [HttpPost("profile/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateProfileComment([FromBody] CreateProfileCommentRequest request)
    {
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var sender = await _authenticationService.GetUser(nameIdentifier);
            var profileComment = await _commentService.CreateProfileComment(request.Comment, request.RecipientId, sender);
            if (profileComment is null)
            {
                return BadRequest(new CreateCommentResponse
                {
                    Success = false,
                    Message = "Comment creation failed"
                });
            }
            return Ok(new CreateCommentResponse
            {
                Success = true,
                Message = "Comment created successfully",
                ProfileComment = profileComment

            });
           
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("profile/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteProfileComment(string id)
    {
        var roles = User.GetRoles();
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            if (await _commentService.DeleteProfileComment(id, roles, user.Id))
                return Ok(new { message = "Comment deleted successfully" });

            return BadRequest(new { message = "You don't have permission to delete this comment" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("song/{id}")]
    public async Task<IActionResult> SongCommentById(string id)
    {
        try
        {
            var comment = await _commentService.GetSongCommentById(id);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("song/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateSongComment([FromBody] CreateSongCommentRequest request)
    {
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            var songComment = await _commentService.CreateSongComment(request.Comment, request.SongId, user);
            if (songComment != null)
                return Ok(new CreateSongCommentResponse
                {
                    Success = true,
                    Message = "Comment created successfully",
                    SongComment = songComment
                });
            return BadRequest(new CreateCommentResponse
            {
                Success = false,
                Message = "Comment creation failed"
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("song/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteSongComment(string id)
    {
        var roles = User.GetRoles();
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            if (await _commentService.DeleteSongComment(id, roles, user.Id))
                return Ok(new { message = "Comment deleted successfully" });

            return BadRequest(new { message = "You don't have permission to delete this comment" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("album/{id}")]
    public async Task<IActionResult> AlbumCommentById(string id)
    {
        try
        {
            var comment = await _commentService.GetAlbumCommentById(id);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("album/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateAlbumComment([FromBody] CreateAlbumCommentRequest request)
    {
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            var albumComment = await _commentService.CreateAlbumComment(request.Comment, request.AlbumId, user);
            if (albumComment != null)
                return Ok(new CreateAlbumCommentResponse
                {
                    Success = true,
                    Message = "Comment created successfully",
                    AlbumComment = albumComment
                });
            return BadRequest(new CreateCommentResponse
            {
                Success = false,
                Message = "Comment creation failed"
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("album/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAlbumComment(string id)
    {
        var roles = User.GetRoles();
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            if (await _commentService.DeleteAlbumComment(id, roles, user.Id))
                return Ok(new { message = "Comment deleted successfully" });

            return BadRequest(new { message = "You don't have permission to delete this comment" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("artist/{id}")]
    public async Task<IActionResult> ArtistCommentById(string id)
    {
        try
        {
            var comment = await _commentService.GetArtistCommentById(id);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("artist/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateArtistComment([FromBody] CreateArtistCommentRequest request)
    {
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            var artistComment = await _commentService.CreateArtistComment(request.Comment, request.ArtistId, user);
            if (artistComment != null)
                return Ok(new CreateArtistCommentResponse
                {
                    Success = true,
                    Message = "Comment created successfully",
                    ArtistComment = artistComment
                });
            return BadRequest(new CreateCommentResponse
            {
                Success = false,
                Message = "Comment creation failed"
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("artist/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteArtistComment(string id)
    {
        var roles = User.GetRoles();
        try
        {
            var nameIdentifier = User.GetNameIdentifier();
            var user = await _authenticationService.GetUser(nameIdentifier);
            if (await _commentService.DeleteArtistComment(id, roles, user.Id))
                return Ok(new { message = "Comment deleted successfully" });

            return BadRequest(new { message = "You don't have permission to delete this comment" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPatch("{subject}/{id}")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditComment(string subject, string id, [FromBody] string newContent)
    {
        try
        {
            var success = await _commentService.EditComment(subject, id, newContent);
            return Ok(new { Success = success });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}