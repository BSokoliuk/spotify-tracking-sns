using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Helpers;
using Services.Interfaces;
using DTOs;
using Results;
using DTOs.Follows;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class FollowController(IFollowService followService) : ControllerBase
{
    private readonly IFollowService _followService = followService;

    [HttpPost("{targetUserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> FollowUser(string targetUserId)
    {
        var sourceUserId = User.GetUserId();

        var result = await _followService.FollowUser(sourceUserId, targetUserId);
        
        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new ApiResponse(false, result.Error.Message));
            
            return BadRequest(new ApiResponse(false, result.Error.Message));
        }

        return Ok(new ApiResponse(true, "Followed successfully"));
    }

    [HttpDelete("{targetUserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> UnfollowUser(string targetUserId)
    {
        var sourceUserId = User.GetUserId();

        var result = await _followService.UnfollowUser(sourceUserId, targetUserId);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new ApiResponse(false, result.Error.Message));
            
            return BadRequest(new ApiResponse(false, result.Error.Message));
        }

        return Ok(new ApiResponse(true, "Unfollowed successfully"));
    }

    [HttpGet("{targetUserId}/is-following")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> IsFollowing(string targetUserId)
    {
        var sourceUserId = User.GetUserId();
        var result = await _followService.IsFollowing(sourceUserId, targetUserId);
        
        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new ApiResponse(false, result.Error.Message));
            
            return BadRequest(new ApiResponse(false, result.Error.Message));
        }

        var isFollowing = result.Value;
        return Ok(new IsFollowingResponse
        {
            Success = true,
            Message = "Followed retrieved successfully",
            IsFollowing = isFollowing
        });
    }
}