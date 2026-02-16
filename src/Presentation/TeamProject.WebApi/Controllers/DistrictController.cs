using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.DTOs.DistrictDTOs;
using TeamProject.Domain.Constants;

namespace TeamProject.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DistrictController : ControllerBase
{
    private readonly IDistrictService _districtService;

    public DistrictController(IDistrictService districtService)
    {
        _districtService = districtService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _districtService.GetAllDistrictsResponse(ct);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken ct)
    {
        var result = await _districtService.GetDistrictsByNameAsync(name, ct);
        return Ok(result);
    }

    [HttpPost("by-ids")]
    public async Task<IActionResult> GetByIds([FromBody] List<int> ids, CancellationToken ct)
    {
        var result = await _districtService.GetDistrictsByIdsAsync(ids, ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.ManageCities)]
    public async Task<IActionResult> Create([FromBody] DistrictCreateRequestDto request, CancellationToken ct)
    {
        var result = await _districtService.CreateDistrictAsync(request, ct);
        if (!result)
            return BadRequest("...");

        return Ok("District created successfully.");
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Policies.ManageCities)]
    public async Task<IActionResult> Update(int id, [FromBody] DistrictUpdateRequestDto request, CancellationToken ct)
    {
        var result = await _districtService.UpdateDistrictAsync(request, id, ct);
        if (!result)
            return BadRequest("Update failed. District not found or validation error (duplicate name/invalid city).");

        return Ok("District updated successfully.");
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.ManageCities)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _districtService.DeleteDistrictAsync(id, ct);
        if (!result)
            return BadRequest("District not found or cannot be deleted (check dependencies).");

        return NoContent(); 
    }


}
