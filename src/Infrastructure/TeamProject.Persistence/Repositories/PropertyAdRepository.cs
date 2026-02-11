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
    public PropertyAdRepository(TeamProjectDbContext context):base(context)
    {
        
    }
}
