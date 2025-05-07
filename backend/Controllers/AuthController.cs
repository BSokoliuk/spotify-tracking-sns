using DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Services.Interfaces;
using Settings;
using Helpers;
using Microsoft.Extensions.Options;
using Results;

namespace Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthenticationService authenticationService,
    IUserService userService,
    IOptions<CookieSettings> cookieSettings) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserService _userService = userService;
    private readonly IOptions<CookieSettings> _cookieSettings = cookieSettings;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var result = await _authenticationService.Register(registerRequest);

        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.ValidationErrorCode)
                return BadRequest(new { Success = false, Error = result.Error.Message });

            return Conflict(new { Success = false, Error = result.Error.Message });
        }

        return Ok(new { Success = true, Message = result.Value });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await _authenticationService.Authenticate(loginRequest);

        if (result.IsFailure)
        {
            return Unauthorized(new { Success = false, Error = result.Error!.Message });
        }

        // Append the JWT token as a cookie
        Response.AppendJwtCookie(result.Value.Token, _cookieSettings.Value);

        return Ok(new
        {
            Success = true,
            result.Value.Id,
            result.Value.IsAdmin,
            result.Value.Token
        });
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout()
    {
        var result = await _authenticationService.Logout(Response, _cookieSettings.Value);

        if (result.IsFailure)
        {
            return BadRequest(new { Success = false, Error = result.Error!.Message });
        }

        return Ok(new { Success = true, Message = result.Value });
    }

    [HttpPost("change-password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.GetUserId();
        var result = await _authenticationService.ChangePassword(userId, request.OldPassword, request.NewPassword);

        if (result.IsFailure)
        {
            return BadRequest(new { Success = false, Error = result.Error!.Message });
        }

        return Ok(new { Success = true, Message = result.Value });
    }

    [HttpGet("user")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult GetUser()
    {
        var userId = User.GetUserId();
        var id = _userService.GetById(userId).Result!.Id;
        bool isAdmin = User.IsAdmin();

        return Ok(new
        {
            Success = true,
            Id = id,
            IsAdmin = isAdmin
        });
    }
}