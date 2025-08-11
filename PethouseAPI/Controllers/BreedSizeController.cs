using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PethouseAPI.Entities;
using PethouseAPI.Services;

namespace PethouseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BreedSizeController(IRepository<BreedSize> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BreedSize>>> GetBreedSize()
    {
        var breedSizes = await repository.GetAll().ToListAsync();
        
        return Ok(breedSizes);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BreedSize>> GetBreedSizeById(int id)
    {
        var result = await repository.GetByIdAsync(id);
        
        if (result is not null)
            return Ok(result);
        
        return BadRequest();
    }
}