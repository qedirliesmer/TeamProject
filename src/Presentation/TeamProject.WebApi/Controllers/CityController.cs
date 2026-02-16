using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.DTOs.CityDTOs;
using TeamProject.Domain.Constants;
using TeamProject.Domain.Entities;

namespace TeamProject.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _cityService.GetAllCitiesResponse(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var cities = await _cityService.GetCitiesByIdsAsync(new List<int> { id }, ct);
        var city = cities.FirstOrDefault();

        if (city == null) return NotFound("The city is not found");
        return Ok(city);
    }
    
    [HttpPost]
    [Authorize(Policy = Policies.ManageCities)]
    public async Task<IActionResult> Create(CityCreateRequestDto request, CancellationToken ct)
    {
        var result = await _cityService.CreateAsync(request, ct);
        if (!result) return BadRequest("The city is not created (The name is already exits).");

        return StatusCode(201, "The city is created successfully.");
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.ManageCities)]
    public async Task<IActionResult> Update(int id, CityUpdateRequestDto request, CancellationToken ct)
    {
        var result = await _cityService.UpdateCityAsync(request, id, ct);
        if (!result) return BadRequest("There was error when updated");

        return Ok("The information is updated");
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.ManageCities)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _cityService.DeleteCityAsync(id, ct);
        if (!result) return NotFound("There is not found city to delete");

        return Ok("The city is deleted");
    }
}

