using Microsoft.EntityFrameworkCore;
using ShopSphere.Application.Common.Interfaces;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence;

public class EfRepository<T> : IGenericRepository<T>
    where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _entities;

    public EfRepository(AppDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _entities
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _entities
            .FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        var existingEntity = await _entities.FindAsync(entity.Id);

        if (existingEntity is null)
        {
            return false;
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);

        await _context.SaveChangesAsync();

        return true;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _entities.FindAsync(id);

        if (entity is null)
        {
            return false;
        }

        _entities.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _entities
            .AnyAsync(e => e.Id == id);
    }
}