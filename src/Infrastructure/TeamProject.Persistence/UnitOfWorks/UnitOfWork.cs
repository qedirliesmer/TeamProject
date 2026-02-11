using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.UnitOfWorks;
using TeamProject.Persistence.Contexts;

namespace TeamProject.Persistence.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly TeamProjectDbContext _context;

    public UnitOfWork(TeamProjectDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
