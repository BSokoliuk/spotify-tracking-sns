using System.Linq.Expressions;

namespace Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[]? includes);
    Task<IEnumerable<T>> FindAllAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? take = null,
        params Expression<Func<T, object>>[]? includes);

    Task<IEnumerable<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    );
    Task<IEnumerable<TResult>> GroupByAsync<TKey, TResult>(
       Expression<Func<T, bool>> predicate,
        Expression<Func<T, TKey>> groupBySelector,
        Expression<Func<IGrouping<TKey, T>, TResult>> resultSelector,
        Func<IQueryable<IGrouping<TKey, T>>, IOrderedQueryable<IGrouping<TKey, T>>>? orderBy = null,
        int? take = null,
        params Expression<Func<T, object>>[]? includes
    );
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountDistinctAsync<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> selector);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(object id);
    void Delete(T entity);
}