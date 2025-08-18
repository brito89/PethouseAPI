using Microsoft.EntityFrameworkCore;
using PethouseAPI.Data;
using PethouseAPI.Entities;

namespace PethouseAPI.Services;

public class PeakSeasonRepository(PethouseDbContext context) : IRepository<PeakSeason>
{
    public IQueryable<PeakSeason> GetAll()
    {
        return context.PeakSeasons.AsNoTracking();
    }

    public async Task<PeakSeason> GetByIdAsync(int id)
    {
        var result = await context.PeakSeasons.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException("Id does not exist");
        
        return result;
    }

    public async Task AddAsync(PeakSeason entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        await context.PeakSeasons.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PeakSeason entity)
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
        context.PeakSeasons.Remove(result);
        await context.SaveChangesAsync();

    }
}