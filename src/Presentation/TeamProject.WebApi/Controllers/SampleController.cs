using Microsoft.AspNetCore.Mvc;

namespace TeamProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Web Api is working!");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new
        {
            Id = id,
            Message = "The ID you sent has been accepted."
        });
    }

    [HttpPost]
    public IActionResult Post([FromBody] string value)
    {
        return Ok(new
        {
            Result = "Information received.",
            Data = value
        });
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] string value)
    {
        return Ok(new
        {
            Id = id,
            UpdatedValue = value
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Deletable(int id)
    {
        return Ok($"ID {id} has been deleted.");
    }
}