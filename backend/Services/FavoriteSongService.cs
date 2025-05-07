using DTOs;
using Models;
using Results;
using Services.Interfaces;
using UoW;

namespace Services;

public class FavoriteSongService(IUnitOfWork unitOfWork) : IFavoriteSongService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CustomResult<ApiResponse>> AddFavoriteSong(string songId, string userId)
    {
        // fetch user by id
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        if (user is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.RecordNotFound("User not found"));
        }

        // fetch song by id
        var song = await _unitOfWork.Repository<Song>().GetByIdAsync(songId);
        if (song is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.RecordNotFound("Song not found"));
        }

        // check if song is already in favourites
        var alreadyAdded = await _unitOfWork.FavoriteSongRepository.FindByUserAndSongAsync(userId, songId) != null;
        if (alreadyAdded)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.ValidationError("Song already added to favorites"));
        }

        // add song to favourites
        var favoriteSong = new FavouriteSong
        {
            Id = Guid.NewGuid().ToString(),
            Id_Song_Internal = songId,
            Id_User = userId,
            User = user,
            Song = song
        };

        await _unitOfWork.FavoriteSongRepository.AddAsync(favoriteSong);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<ApiResponse>.Success(new ApiResponse(true, "Song added to favorites"));
    }

    public async Task<CustomResult<ApiResponse>> DeleteFavoriteSong(string songId, string userId)
    {
        // fetch user by id
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        if (user is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.RecordNotFound("User not found"));
        }

        // fetch song by id
        var song = await _unitOfWork.Repository<Song>().GetByIdAsync(songId);
        if (song is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.RecordNotFound("Song not found"));
        }

        // check if song is already in favorites
        var favoriteSong = await _unitOfWork.FavoriteSongRepository.FindByUserAndSongAsync(userId, songId);
        if (favoriteSong is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.ValidationError("Cannot delete song from favorites, song not found in favorites"));
        }

        // remove song from favorites
        _unitOfWork.FavoriteSongRepository.Delete(favoriteSong);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<ApiResponse>.Success(new ApiResponse(true, "Song removed from favorites"));
    }

    public async Task<List<FavouriteSong>> GetMostLikedSongs(int count)
    {
        return await _unitOfWork.FavoriteSongRepository.GetMostLikedSongs(count);
    }
}