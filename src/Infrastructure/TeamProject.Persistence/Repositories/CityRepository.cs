using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;
using TeamProject.Persistence.Contexts;

namespace TeamProject.Persistence.Repositories;

public class CityRepository : GenericRepository<City, int>, ICityRepository
{
    private readonly TeamProjectDbContext _context;
    public CityRepository(TeamProjectDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByNameAsync(string name, int excludeId, CancellationToken ct)
    {
        return await _context.Cities.AnyAsync(c=>c.Name ==name&& c.Id!= excludeId, ct);
    }
}
