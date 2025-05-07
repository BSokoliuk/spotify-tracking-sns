using Data;
using Repositories;
using Repositories.Interfaces;

namespace UoW;

public sealed class UnitOfWork(DatabaseContext context) : IUnitOfWork
{
    private readonly DatabaseContext _context = context;
    private readonly Dictionary<Type, object> _repositories = [];

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new Repository<TEntity>(_context);
            _repositories[type] = repositoryInstance;
        }

        return (IRepository<TEntity>)_repositories[type];
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}