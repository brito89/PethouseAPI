using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PethouseAPI.Entities;
using PethouseAPI.Services;

namespace PethouseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PetController(IRepository<Pet> repository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Pet>> GetAll()
    {
        var result = repository.GetAll()
            .Include(pet => pet.BreedSize)
            .Include(pet => pet.User)
            .ToList();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pet>> GetById(int id)
    {
        try
        {
            var entity = await repository.GetByIdAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Appointment with id {id} not found.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Pet entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await repository.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Pet entity)
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
            return NotFound($"Appointment with id {id} not found.");
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
            return NotFound($"Appointment with id {id} not found.");
        }
    }
}