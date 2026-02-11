using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;
using TeamProject.Persistence.Contexts;

namespace TeamProject.Persistence.Repositories;

public class DistrictRepository : GenericRepository<District, int>, IDistrictRepository
{
    public DistrictRepository(TeamProjectDbContext context) : base(context)
    {
    }
}
