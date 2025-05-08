using Repositories.Interfaces;

namespace UoW;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    IFavoriteSongRepository FavoriteSongRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}