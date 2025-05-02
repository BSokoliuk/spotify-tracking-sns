using Microsoft.AspNetCore.Identity;
using DTOs;
using Models;

namespace Services;

public class AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<(bool, string)> Register(RegisterRequest registerRequest)
    {
        var user = new User
        {
            Email = registerRequest.Email,
            UserName = registerRequest.Username
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        var errors = result.Errors.ToList();

        if (result.Succeeded)
            return (true, "");

        return (false, errors[0].Description);
    }

    public async Task<bool> AddRole(string username, string role)
    {
        var user = await _userManager.FindByNameAsync(username)
            ?? throw new Exception("User not found");;
        var result = await _userManager.AddToRoleAsync(user, role);
        
        return result.Succeeded;
    }

    public async Task<List<string>> GetRoles(string username)
    {
        var user = await _userManager.FindByNameAsync(username)
            ?? throw new Exception("User not found");
        var roles = await _userManager.GetRolesAsync(user);

        return [.. roles];
    }

    public async Task<bool> Login(LoginRequest loginRequest)
    {
        var result = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);

        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }

    public async Task<User> GetUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username)
            ?? throw new Exception("User not found");
        return user;
    }

    public static byte[] getDefaultAvatar()
    {
        byte[] imageByte = File.ReadAllBytes("avatar.jpg");
        return imageByte;
    }

    public async Task<(bool, string)> ChangePassword(string username, string oldPassword, string newPassword)
    {
        User user = await _userManager.FindByNameAsync(username)
            ?? throw new Exception("User not found");
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        var errors = result.Errors.ToList();

        if (result.Succeeded)
            return (true, "");

        return (false, errors[0].Description.ToString());
    }
}