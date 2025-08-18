using Microsoft.AspNetCore.Mvc;
using PethouseAPI.Entities;
using PethouseAPI.Services;

namespace PethouseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PetAppointmentController(IRepository<PetAppointment> repository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PetAppointment>> GetAll()
    {
        var result = repository.GetAll().ToList();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PetAppointment>> GetById(int id)
    {
        try
        {
            var entity = await repository.GetByIdAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Pet Appointment with id {id} not found.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PetAppointment entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await repository.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] PetAppointment entity)
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
            return NotFound($"Pet Appointment with id {id} not found.");
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
            return NotFound($"Pet Appointment with id {id} not found.");
        }
    }
    
}