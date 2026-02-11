using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.Abstracts.UnitOfWorks;

public interface IUnitOfWork:IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
