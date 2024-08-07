using System.Linq.Expressions;
using MongoDB.Driver;
using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task AddBulkAsync(List<TEntity> entities);
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(string id);
    Task<List<TEntity>> Filter(FilterDefinition<TEntity> filter);
    Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> RemoveBulkAsync(Expression<Func<TEntity, bool>> predicate);
    Task UpdateAsync(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
}
