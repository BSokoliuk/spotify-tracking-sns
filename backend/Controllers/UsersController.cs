using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Results;
using Services.Interfaces;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("most-active")]
    public async Task<IActionResult> GetMostActiveUsers()
    {
        var result = await _userService.FetchMostActiveUsers();
        return Ok(result);
    }

    [HttpGet("compatibility")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Compatibility([FromQuery] string user_id)
    {
        var userId = User.GetUserId();
        var result = await _userService.CalculateCompatibility(user_id, userId);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.ValidationErrorCode)
                return BadRequest(new { Success = false, Error = result.Error.Message });

            return NotFound(new { Success = false, Error = result.Error.Message });
        }

        return Ok(result.Value);
    }
}