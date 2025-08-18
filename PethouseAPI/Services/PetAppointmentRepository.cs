using Microsoft.EntityFrameworkCore;
using PethouseAPI.Data;
using PethouseAPI.Entities;

namespace PethouseAPI.Services;

public class PetAppointmentRepository(PethouseDbContext  context) : IRepository<PetAppointment>
{
    public IQueryable<PetAppointment> GetAll()
    {
        return context.PetAppointments.AsNoTracking();
    }

    public async Task<PetAppointment> GetByIdAsync(int id)
    {
        var result = await context.PetAppointments.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException("Id does not exist");
        
        return result;
    }

    public async Task AddAsync(PetAppointment entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        await context.PetAppointments.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PetAppointment entity)
    {
        var existing = await GetByIdAsync(entity.Id);
        
        if (existing is null)
            throw new KeyNotFoundException($"PetAppointment with ID {entity.Id} not found.");

        context.Entry(existing).CurrentValues.SetValues(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var result = await GetByIdAsync(id); // Will throw if not found
        context.PetAppointments.Remove(result);
        await context.SaveChangesAsync();

    }
}