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

public class PropertyAdRepository : GenericRepository<PropertyAd, int>, IPropertyAdRepository
{
       private readonly TeamProjectDbContext _context;

    public PropertyAdRepository(TeamProjectDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PropertyAd?> GetWithDetailsAsync(int id)
    {
        return await _context.PropertyAds
            .Include(x => x.User)       
            .Include(x => x.MediaItems) 
            .FirstOrDefaultAsync(x => x.Id == id);
    }

}

