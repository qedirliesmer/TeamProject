using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.CityDTOs;

namespace TeamProject.Application.Abstracts.Services;

public interface ICityService
{
    Task<bool> CreateAsync(CityCreateRequestDto request, CancellationToken ct=default);
    Task<bool> DeleteCityAsync(int Id, CancellationToken ct=default);
    Task<bool> UpdateCityAsync(CityUpdateRequestDto request, int Id, CancellationToken ct=default);
    Task<List<GetAllCitiesResponse>> GetAllCitiesResponse(CancellationToken ct=default);
    Task<List<GetAllCitiesResponse>> GetCitiesByIdsAsync(List<int>ids , CancellationToken ct = default);
    Task<List<GetAllCitiesResponse>> GetCitiesByNameAsync(string Name, CancellationToken ct = default);
}
