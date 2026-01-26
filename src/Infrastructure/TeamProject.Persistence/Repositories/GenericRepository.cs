
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
    public void Add(TEntity entity)
    {
        _table.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        _table.Remove(entity);
    }

    public List<TEntity> GetAll()
    {
        return _table.ToList();
    }

    public TEntity GetById(TKey id)
    {
        return _table.Find(id);
    }

    public void Update(TEntity entity)
    {
        _table.Update(entity);
    }
    public void SaveChanges()
    {
       _context.SaveChanges();
    }

}
