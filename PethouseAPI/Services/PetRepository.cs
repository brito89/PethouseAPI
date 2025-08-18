using Microsoft.EntityFrameworkCore;
using PethouseAPI.Data;
using PethouseAPI.Entities;

namespace PethouseAPI.Services;

public class PetRepository(PethouseDbContext context) : IRepository<Pet>
{
    public IQueryable<Pet> GetAll()
    {
        return context.Pets.AsNoTracking();
    }

    public async Task<Pet> GetByIdAsync(int id)
    {
        var result = await context.Pets.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException("Id does not exist");
        
        return result;
    }

    public async Task AddAsync(Pet entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        await context.Pets.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pet entity)
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
        context.Pets.Remove(result);
        await context.SaveChangesAsync();
    }
}