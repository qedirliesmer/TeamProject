using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.PropertyAdDTOs;

namespace TeamProject.Application.Abstracts.Services;

public interface IPropertyAdService
{
    Task CreateAsync(PropertyAdCreateDto dto, CancellationToken ct);
    Task UpdateAsync(PropertyAdUpdateDto input, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task<List<PropertyAdGetAllDto>> GetAllAsync();
    Task<PropertyAdGetByIdDto> GetByIdAsync(int id);
}
