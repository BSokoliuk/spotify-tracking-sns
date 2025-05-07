using DTOs;
using DTOs.Users;
using Models;
using Results;

namespace Services.Interfaces;

public interface IUserService
{
  Task<User?> GetByUserName(string username);
  Task<User?> GetById(string id);
  Task<CustomResult<CompatibilityResponse>> CalculateCompatibility(string userId, string senderId);
  Task<List<MostActiveUsers>> FetchMostActiveUsers();
}