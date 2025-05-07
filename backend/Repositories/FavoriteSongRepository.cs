using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Repositories;

public class FavoriteSongRepository(DbContext context) : Repository<FavouriteSong>(context), IFavoriteSongRepository
{
  private readonly DbSet<FavouriteSong> _dbSet = context.Set<FavouriteSong>();
  public async Task<FavouriteSong?> FindByUserAndSongAsync(string userId, string songId)
  {
      return await _dbSet.FirstOrDefaultAsync(fs => fs.Id_User == userId && fs.Id_Song_Internal == songId);
  }

  public async Task<List<FavouriteSong>> GetMostLikedSongs(int count)
  {
      return await _dbSet
          .Include(fs => fs.Song)
          .ThenInclude(s => s.Album)
          .ThenInclude(a => a.Artist)
          .Include(fs => fs.User)
          .OrderByDescending(fs => fs.User.Followers.Count)
          .Take(count)
          .ToListAsync();
  }
}