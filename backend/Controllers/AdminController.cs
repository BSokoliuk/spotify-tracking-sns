using DTOs;
using DTOs.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Results;
using Services.Interfaces;

namespace Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController(IProfileService profileService) : ControllerBase
{
  private readonly IProfileService _profileService = profileService;

  [HttpPatch("bio")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<ApiResponse>> ChangeBio([FromBody] ChangeBioAdminRequest request)
  {
    var result = await _profileService.ChangeBio(request.UserId, request.EditedBio);
    
    if (result.IsFailure)
    {
      if (result.Error!.Code == CustomError.NotFoundCode)
        return NotFound(new ApiResponse(false, result.Error.Message));

      return BadRequest(new ApiResponse(false, result.Error.Message));
    }
    
    return Ok(result.Value);
  }

  [HttpPatch("avatar")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<ApiResponse>> ChangeAvatar([FromBody] ChangeAvatarAdminRequest request)
  {
    var result = await _profileService.ChangeAvatar(request.UserId, request.Avatar);
    
    if (result.IsFailure)
    {
      if (result.Error!.Code == CustomError.NotFoundCode)
        return NotFound(new ApiResponse(false, result.Error.Message));
      
      return BadRequest(new ApiResponse(false, result.Error.Message));
    }

    return Ok(result.Value);
  }
}