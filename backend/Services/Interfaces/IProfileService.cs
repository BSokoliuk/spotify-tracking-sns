using DTOs;
using Results;

namespace Services.Interfaces;

public interface IProfileService
{
  public Task<CustomResult<ProfileResponse>> GetProfileData(string username);
  public Task<CustomResult<ApiResponse>> ChangeBio(string userId, string bio);
  public Task<CustomResult<ApiResponse>> ChangeAvatar(string userId, string avatar);
}