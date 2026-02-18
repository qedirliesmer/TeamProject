using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Application.Abstracts.Repositories;

public interface IPropertyAdRepository: IRepository<PropertyAd, int>
{
    Task<PropertyAd?> GetWithDetailsAsync(int id);
}
