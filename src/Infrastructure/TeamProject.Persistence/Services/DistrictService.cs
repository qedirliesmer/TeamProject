using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.DTOs.DistrictDTOs;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Services;

public class DistrictService : IDistrictService
{
    private readonly IDistrictRepository _districtRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IValidator<DistrictCreateRequestDto> _createValidator;
    private readonly IValidator<DistrictUpdateRequestDto> _updateValidator;

    public DistrictService(IDistrictRepository districtRepository, ICityRepository cityRepository, IValidator<DistrictCreateRequestDto> createValidator, IValidator<DistrictUpdateRequestDto> updateValidator)
    {
        _districtRepository = districtRepository;
        _cityRepository = cityRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<bool> CreateDistrictAsync(DistrictCreateRequestDto request, CancellationToken ct = default)
    {
        var validationResult = await _createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return false;

        var cityExists = await _cityRepository.GetByIdAsync(request.CityId);
        if (cityExists == null) return false;

        var isDuplicate = await _districtRepository.GetAll()
            .AnyAsync(x => x.CityId == request.CityId &&
                           x.Name.ToLower() == request.Name.Trim().ToLower(), ct);

        if (isDuplicate) return false;

        var district = new District
        {
            Name = request.Name.Trim(),
            CityId = request.CityId
        };

        await _districtRepository.AddAsync(district);
        await _districtRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteDistrictAsync(int Id, CancellationToken ct = default)
    {
        var district = await _districtRepository.GetByIdAsync(Id);

        if (district == null)
        {
            return false;
        }

        _districtRepository.Delete(district);

        try
        {
            await _districtRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<GetAllDistrictsResponse>> GetAllDistrictsResponse(CancellationToken ct = default)
    {
        var districts = await _districtRepository.GetAll()

        .AsNoTracking()
        .Include(x => x.City)

        .Select(x => new GetAllDistrictsResponse
        {
            Id = x.Id,
            Name = x.Name,
            CityId = x.CityId,
            CityName = x.City != null ? x.City.Name : "N/A"
        })
        .ToListAsync(ct);

        return districts;
    }

    public async Task<List<GetAllDistrictsResponse>> GetDistrictsByIdsAsync(List<int> ids, CancellationToken ct = default)
    {
        if (ids == null || !ids.Any())
        {
            return new List<GetAllDistrictsResponse>();
        }
        var districts = await _districtRepository.GetAll()
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id)) 
            .Include(x => x.City) 
            .Select(x => new GetAllDistrictsResponse
            {
                Id = x.Id,
                Name = x.Name,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.Name : "N/A"
            })
            .ToListAsync(ct);

        return districts;
    }

    public async Task<List<GetAllDistrictsResponse>> GetDistrictsByNameAsync(string Name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return new List<GetAllDistrictsResponse>();
        }

        var searchTerm = Name.Trim().ToLower();

        var districts = await _districtRepository.GetAll()
            .AsNoTracking()
            .Include(x => x.City)
            .Where(x => x.Name.ToLower().Contains(searchTerm))
            .Select(x => new GetAllDistrictsResponse
            {
                Id = x.Id,
                Name = x.Name,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.Name : "N/A"
            })
            .ToListAsync(ct);

        return districts;
    }

    public async Task<bool> UpdateDistrictAsync(DistrictUpdateRequestDto request, int Id, CancellationToken ct = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return false;

        var district = await _districtRepository.GetByIdAsync(Id);
        if (district == null) return false;

        if (district.CityId != request.CityId)
        {
            var cityExists = await _cityRepository.GetByIdAsync(request.CityId);
            if (cityExists == null) return false;
        }

        var isDuplicate = await _districtRepository.GetAll()
            .AnyAsync(x => x.CityId == request.CityId &&
                           x.Name.ToLower() == request.Name.Trim().ToLower() &&
                           x.Id != Id, ct);

        if (isDuplicate) return false;

        district.Name = request.Name.Trim();
        district.CityId = request.CityId;

        _districtRepository.Update(district);
        await _districtRepository.SaveChangesAsync();

        return true;
    }
}
