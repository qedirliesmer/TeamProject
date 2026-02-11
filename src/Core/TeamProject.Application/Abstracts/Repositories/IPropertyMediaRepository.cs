using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Application.Abstracts.Repositories;

public interface IPropertyMediaRepository : IRepository<PropertyMedia,int>
{
    Task<List<PropertyMedia>> GetByPropertyAdIdAsync(int propertyAdId, CancellationToken ct);
}
