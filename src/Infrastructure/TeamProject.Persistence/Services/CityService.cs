using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.DTOs.CityDTOs;
using TeamProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace TeamProject.Persistence.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IValidator<CityCreateRequestDto> _createValidator;
    private readonly IValidator<CityUpdateRequestDto> _updateValidator;

    public CityService(ICityRepository cityRepository, IValidator<CityCreateRequestDto> createValidator, IValidator<CityUpdateRequestDto> updateValidator)
    {
        _cityRepository = cityRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<bool> CreateAsync(CityCreateRequestDto request, CancellationToken ct = default)
    {
        var exists = await _cityRepository.ExistsByNameAsync(request.Name, 0, ct);
        if (exists)
            return false;


        var city = new City
        {
            Name = request.Name,
            CreatedAt = request.CreatedAt == default
                ? DateTime.UtcNow
                : request.CreatedAt
        };

        await _cityRepository.AddAsync(city);
        await _cityRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCityAsync(int Id, CancellationToken ct = default)
    {
        var city = await _cityRepository.GetByIdAsync(Id);
        if (city is null)
            return false;

         _cityRepository.Delete(city);
        await _cityRepository.SaveChangesAsync();

        return true;
    }

    public async Task<List<GetAllCitiesResponse>> GetAllCitiesResponse(CancellationToken ct = default)
    {
        var cities =  _cityRepository.GetAll();

        return cities.Select(c => new GetAllCitiesResponse
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<List<GetAllCitiesResponse>> GetCitiesByIdsAsync(List<int> ids, CancellationToken ct = default)
    {
        if (ids == null || ids.Count == 0)
            return new List<GetAllCitiesResponse>();

        var cities =  _cityRepository.GetAll();

        return cities
            .Where(c => ids.Contains(c.Id))
            .Select(c => new GetAllCitiesResponse
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt
            })
            .ToList();
    }

    public async Task<List<GetAllCitiesResponse>> GetCitiesByNameAsync(string name, CancellationToken ct = default)
    {
        name = (name ?? "").Trim();
        if (name.Length == 0)
            return new List<GetAllCitiesResponse>();

        var cities = _cityRepository.GetAll();

        return cities
            .Where(c => c.Name.Contains(name))
            .Select(c => new GetAllCitiesResponse
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt
            })
            .ToList();
    }

    public async Task<bool> UpdateCityAsync(CityUpdateRequestDto request, int Id, CancellationToken ct = default)
    {
        var city = await _cityRepository.GetByIdAsync(Id);
        if (city is null)
            return false;


        var exists = await _cityRepository.ExistsByNameAsync(
            request.Name,
            Id,
            ct
        );

        if (exists)
            return false;

        city.Name = request.Name;
        city.UpdatedAt = request.UpdatedAt == default
            ? DateTime.UtcNow
            : request.UpdatedAt;

         _cityRepository.Update(city);
        await _cityRepository.SaveChangesAsync();

        return true;
    }
}
