
using Microsoft.EntityFrameworkCore;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;
using TeamProject.Persistence.Contexts;

namespace TeamProject.Persistence.Repositories;

public class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    private readonly TeamProjectDbContext _context;
    private readonly DbSet<TEntity> _table;

    public GenericRepository(TeamProjectDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _table.AsNoTracking();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _table.AddAsync(entity);
    }

    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        return await _table.FindAsync(id);
    }

    public void Update(TEntity entity)
    {
        _table.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _table.Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
