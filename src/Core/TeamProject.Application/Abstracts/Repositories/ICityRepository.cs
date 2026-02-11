using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Application.Abstracts.Repositories;

public interface ICityRepository : IRepository<City, int>
{
    Task<bool> ExistsByNameAsync(string name, int excludeId, CancellationToken ct);
}
