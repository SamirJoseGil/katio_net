using katio.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace katio.Data;

public class Repository<TId, TEntity> : IRepository<TId, TEntity>
where TId : struct
where TEntity : BaseEntity<TId>
{
    internal katioContext _context;
    internal DbSet<TEntity> _dbSet;

    public Repository(katioContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> FindAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public async Task Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(TId id)
    {
        throw new NotImplementedException();
    }
    
    public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, Book>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null, string includeProperties = "")
    {
        throw new NotImplementedException();
    }
}
