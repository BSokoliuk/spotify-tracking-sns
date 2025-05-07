using Microsoft.AspNetCore.Identity;
using DTOs.Auth;
using Models;
using Services.Interfaces;
using Settings;
using Helpers;
using Results;

namespace Services;

public class AuthenticationService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IUserService userService,
    ITokenService tokenService) : IAuthenticationService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<CustomResult<string>> Register(RegisterRequest registerRequest)
    {
        var user = new User
        {
            Email = registerRequest.Email,
            UserName = registerRequest.Username
        };

        // Create the user
        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
        {
            return CustomResult<string>.Failure(GetIdentityError(result));
        }

        // Assign the default role
        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            return CustomResult<string>.Failure(GetIdentityError(roleResult));
        }

        return CustomResult<string>.Success("User registered successfully");
    }

    public async Task<CustomResult<LoginResponse>> Authenticate(LoginRequest loginRequest)
    {
        // Validate credentials
        var result = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);
        if (!result.Succeeded)
        {
            return CustomResult<LoginResponse>.Failure(CustomError.ValidationError("Invalid credentials"));
        }

        // Fetch user and roles
        var user = await _userService.GetByUserName(loginRequest.Username);

        if (user is null)
        {
            return CustomResult<LoginResponse>.Failure(CustomError.RecordNotFound("User not found"));
        }

        var roles = await _userManager.GetRolesAsync(user);

        // Generate token
        var token = _tokenService.Generate(user, [.. roles]);

        return CustomResult<LoginResponse>.Success(new LoginResponse
        {
            Id = user.Id,
            IsAdmin = roles.Contains("Admin"),
            Token = token
        });
    }

    public async Task<CustomResult<string>> Logout(HttpResponse response, CookieSettings cookieSettings)
    {
        await _signInManager.SignOutAsync();
        response.DeleteJwtCookie(cookieSettings);
        return CustomResult<string>.Success("Logout successful");
    }

    public async Task<CustomResult<string>> ChangePassword(string id, string oldPassword, string newPassword)
    {
        // Find the user
        var user = await _userService.GetById(id);
        if (user == null)
        {
            return CustomResult<string>.Failure(CustomError.RecordNotFound("User not found"));
        }

        // Attempt to change the password
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            return CustomResult<string>.Failure(GetIdentityError(result));
        }

        return CustomResult<string>.Success("Password was successfully changed");
    }

    private static CustomError GetIdentityError(IdentityResult result)
    {
        var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "An unknown error occurred";
        return CustomError.ValidationError(errorMessage);
    }
}