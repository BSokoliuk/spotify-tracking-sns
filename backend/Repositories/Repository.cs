using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet.Where(predicate);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    )
    {
        IQueryable<T> query = _dbSet.Where(predicate);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? take = null,
        params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet.Where(predicate);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TResult>> GroupByAsync<TKey, TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TKey>> groupBySelector,
        Expression<Func<IGrouping<TKey, T>, TResult>> resultSelector,
        Func<IQueryable<IGrouping<TKey, T>>, IOrderedQueryable<IGrouping<TKey, T>>>? orderBy = null,
        int? take = null,
        params Expression<Func<T, object>>[]? includes
    )
    {
        IQueryable<T> query = _dbSet.Where(predicate);

        // Apply includes for eager loading
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply GroupBy
        var groupedQuery = query.GroupBy(groupBySelector);

        // Apply ordering to the grouped query
        if (orderBy != null)
        {
            groupedQuery = orderBy(groupedQuery);
        }

        // Apply take (limit) if specified
        if (take.HasValue)
        {
            groupedQuery = groupedQuery.Take(take.Value);
        }

        // Project the grouped results
        return await groupedQuery.Select(resultSelector).ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    public async Task<int> CountDistinctAsync<TProperty>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TProperty>> selector)
    {
        return await _dbSet.Where(predicate).Select(selector).Distinct().CountAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Delete(object id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
}