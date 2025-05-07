using DTOs;
using Models;
using Results;

namespace Services.Interfaces;

public interface IFavoriteSongService
{
  Task<CustomResult<ApiResponse>> AddFavoriteSong(string songId, string userId);
  Task<CustomResult<ApiResponse>> DeleteFavoriteSong(string songId, string userId);
  Task<List<FavouriteSong>> GetMostLikedSongs(int count);
}