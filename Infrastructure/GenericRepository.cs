using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;

namespace MyWebApi.Infrastructure;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    private readonly IMongoCollection<TEntity> _collection;

    public GenericRepository(IOptions<MongoSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task AddBulkAsync(List<TEntity> entities)
    {
        await _collection.InsertManyAsync(entities);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> Filter(FilterDefinition<TEntity> filter)
    {
        return await _collection.FindSync(filter).ToListAsync();
    }

    public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }

    public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var res = await _collection.DeleteOneAsync(predicate);
        return res.DeletedCount > 0;
    }

    public async Task<bool> RemoveBulkAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var res = await _collection.DeleteManyAsync(predicate);
        return res.DeletedCount > 0;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).AnyAsync();
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.CountDocumentsAsync(predicate);
    }
}
