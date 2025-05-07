using Models;

namespace Repositories.Interfaces;

public interface IFavoriteSongRepository : IRepository<FavouriteSong>
{
    Task<FavouriteSong?> FindByUserAndSongAsync(string userId, string songId);
    Task<List<FavouriteSong>> GetMostLikedSongs(int count);
}