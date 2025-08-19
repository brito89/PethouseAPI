using Microsoft.AspNetCore.Mvc;
using PethouseAPI.Entities;
using PethouseAPI.Services;

namespace PethouseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IRepository<User> repository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        var result = repository.GetAll().ToList();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        try
        {
            var entity = await repository.GetByIdAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"BreedSize with id {id} not found.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] User entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await repository.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] User entity)
    {
        if (id != entity.Id)
            return BadRequest("ID mismatch.");

        try
        {
            await repository.UpdateAsync(entity);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"BreedSize with id {id} not found.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await repository.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"BreedSize with id {id} not found.");
        }
    }
}