using DTOs.Spotify;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Helpers;
using Services.Interfaces;
using Results;
using DTOs;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpotifyController(ISpotifyService spotifyService) : ControllerBase
{
    private readonly ISpotifyService _spotifyService = spotifyService;

    [HttpPost("connect")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApiResponse>> ConnectSpotify([FromBody] ConnectSpotifyRequest request)
    {
        var userId = User.GetUserId();
        var result = await _spotifyService.ConnectSpotify(request, userId);

        return HandleResult(result);
    }

    [HttpPatch("disconnect")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApiResponse>> DisconnectSpotify()
    {
        var userId = User.GetUserId();
        var result = await _spotifyService.DisconnectSpotify(userId);

        return HandleResult(result);
    }

    private ActionResult<ApiResponse> HandleResult(CustomResult<ApiResponse> result)
    {
        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.NotFoundCode)
                return NotFound(new { Success = false, Error = result.Error.Message });

            return BadRequest(new { Success = false, Error = result.Error.Message });
        }

        return Ok(result.Value);
    }
}