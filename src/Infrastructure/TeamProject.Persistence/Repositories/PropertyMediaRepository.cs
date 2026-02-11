using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;
using TeamProject.Persistence.Contexts;

namespace TeamProject.Persistence.Repositories;

public class PropertyMediaRepository : GenericRepository<PropertyMedia, int>, IPropertyMediaRepository
{
    private readonly TeamProjectDbContext _context;

    public PropertyMediaRepository(TeamProjectDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<PropertyMedia>> GetByPropertyAdIdAsync(int propertyAdId, CancellationToken ct)
    {
        return await _context.PropertyMedias
            .Where(x => x.PropertyAdId == propertyAdId) 
            .OrderBy(x => x.Order)         
            .ToListAsync(ct); 
    }
}
