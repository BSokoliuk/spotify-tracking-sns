using DTOs;
using DTOs.Profile;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Results;
using Services.Interfaces;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController(IProfileService profileService) : ControllerBase
{
    private readonly IProfileService _profileService = profileService;

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserProfile(string username)
    {
        var result = await _profileService.GetProfileData(username);
        if (result.IsFailure)
            return NotFound(new { Success = false, result.Error!.Message });
        
        return Ok(result.Value);
    }

    [HttpPatch("bio")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditBio([FromBody] ChangeBioRequest request)
    {
        var userId = User.GetUserId();
        var result = await _profileService.ChangeBio(userId, request.EditedBio);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.ValidationErrorCode)
                return BadRequest(new ApiResponse(false, result.Error.Message));

            return NotFound(new ApiResponse(false, result.Error.Message));
        }

        return Ok(result.Value);
    }

    [HttpPatch("avatar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditAvatar([FromBody] ChangeAvatarRequest request)
    {
        var userId = User.GetUserId();
        var result = await _profileService.ChangeAvatar(userId, request.Avatar);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.ValidationErrorCode)
                return BadRequest(new ApiResponse(false, result.Error.Message));

            return NotFound(new ApiResponse(false, result.Error.Message));
        }

        return Ok(result.Value);
    }
}