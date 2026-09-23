using System.Collections.Concurrent;
using ShopSphere.Application.Common.Interfaces;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence;

public class InMemoryRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ConcurrentDictionary<int, T> _entities = new();
    private int _currentId = 0;

    public virtual Task<IReadOnlyList<T>> GetAllAsync()
    {
        IReadOnlyList<T> list = _entities.Values.ToList().AsReadOnly();
        return Task.FromResult(list);
    }

    public virtual Task<T?> GetByIdAsync(int id)
    {
        _entities.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public virtual Task<T> AddAsync(T entity)
    {
        if (entity.Id <= 0)
        {
            entity.Id = Interlocked.Increment(ref _currentId);
        }
        else if (entity.Id > _currentId)
        {
            Interlocked.Exchange(ref _currentId, entity.Id);
        }

        _entities[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public virtual Task<bool> UpdateAsync(T entity)
    {
        if (!_entities.ContainsKey(entity.Id))
        {
            return Task.FromResult(false);
        }

        _entities[entity.Id] = entity;
        return Task.FromResult(true);
    }

    public virtual Task<bool> DeleteAsync(int id)
    {
        var removed = _entities.TryRemove(id, out _);
        return Task.FromResult(removed);
    }

    public virtual Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_entities.ContainsKey(id));
    }
}
