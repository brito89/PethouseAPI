using Microsoft.EntityFrameworkCore;
using PethouseAPI.Data;
using PethouseAPI.Entities;

namespace PethouseAPI.Services;

public class AppointmentRepository(PethouseDbContext context) : IRepository<Appointment>
{
    public IQueryable<Appointment> GetAll()
    {
        return context.Appointments.AsNoTracking();
    }

    public async Task<Appointment> GetByIdAsync(int id)
    {
        var result = await context.Appointments.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException("Id does not exist");
        
        return result;
    }

    public async Task AddAsync(Appointment entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        await context.Appointments.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment entity)
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
        context.Appointments.Remove(result);
        await context.SaveChangesAsync();

    }
}