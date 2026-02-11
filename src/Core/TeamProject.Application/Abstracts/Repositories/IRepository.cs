using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Application.Abstracts.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    IQueryable<TEntity> GetAll(); 
    Task<TEntity> GetByIdAsync(TKey id);
    Task AddAsync(TEntity entity); 
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<int> SaveChangesAsync(); 
}
