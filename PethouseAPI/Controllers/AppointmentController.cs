using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PethouseAPI.Entities;
using PethouseAPI.Services;

namespace PethouseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController(IRepository<Appointment> repository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Appointment>> GetAll()
    {
        var result = repository.GetAll().ToList();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetById(int id)
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
    public async Task<ActionResult> Create([FromBody] Appointment entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await repository.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Appointment appointment)
    {
        if (id != appointment.Id)
            return BadRequest("ID mismatch.");

        try
        {
            await repository.UpdateAsync(appointment);
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