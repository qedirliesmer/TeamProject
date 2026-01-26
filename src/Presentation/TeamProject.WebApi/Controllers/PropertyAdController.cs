using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;

namespace TeamProject.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyAdController : ControllerBase
{
    private readonly IRepository<PropertyAd, int> _repository;
    public PropertyAdController(IRepository<PropertyAd, int> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var ads = _repository.GetAll();
        return Ok(ads);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var ad = _repository.GetById(id);
        if (ad == null) return NotFound("The annoucement is not found");
        return Ok(ad);
    }

    [HttpPost]
    public IActionResult Create(PropertyAd propertyAd)
    {
        _repository.Add(propertyAd); 
        _repository.SaveChanges();
        return Ok(propertyAd);
    }

    [HttpPut]
    public IActionResult Update(PropertyAd propertyAd)
    {
        _repository.Update(propertyAd);
        _repository.SaveChanges();
        return Ok("The information is updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var ad = _repository.GetById(id);
        if (ad == null) return NotFound();

        _repository.Delete(ad);
        _repository.SaveChanges();
        return Ok("Announcement is deleted");
    }
}
