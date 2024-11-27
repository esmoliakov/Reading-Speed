using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace Server.Database;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly ReadingSpeedDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ReadingSpeedDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id);
    }
    
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(T entity)
    {
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}