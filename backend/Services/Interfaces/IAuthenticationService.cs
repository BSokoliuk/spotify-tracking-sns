using DTOs.Auth;
using Settings;
using Results;

namespace Services.Interfaces;

public interface IAuthenticationService
{
    Task<CustomResult<string>> Register(RegisterRequest registerRequest);
    Task<CustomResult<LoginResponse>> Authenticate(LoginRequest loginRequest);
    Task<CustomResult<string>> Logout(HttpResponse response, CookieSettings cookieSettings);
    Task<CustomResult<string>> ChangePassword(string id, string oldPassword, string newPassword);
}