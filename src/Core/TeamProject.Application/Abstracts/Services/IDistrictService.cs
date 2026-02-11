using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.CityDTOs;
using TeamProject.Application.DTOs.DistrictDTOs;

namespace TeamProject.Application.Abstracts.Services;

public interface IDistrictService
{
    Task<bool> CreateDistrictAsync(DistrictCreateRequestDto request, CancellationToken ct = default);
    Task<bool> DeleteDistrictAsync(int Id, CancellationToken ct = default);
    Task<bool> UpdateDistrictAsync(DistrictUpdateRequestDto request, int Id, CancellationToken ct = default);
    Task<List<GetAllDistrictsResponse>> GetAllDistrictsResponse(CancellationToken ct = default);
    Task<List<GetAllDistrictsResponse>> GetDistrictsByIdsAsync(List<int> ids, CancellationToken ct = default);
    Task<List<GetAllDistrictsResponse>> GetDistrictsByNameAsync(string Name, CancellationToken ct = default);
}
