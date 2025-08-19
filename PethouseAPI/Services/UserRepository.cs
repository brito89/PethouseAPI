using Microsoft.EntityFrameworkCore;
using PethouseAPI.Data;
using PethouseAPI.Entities;

namespace PethouseAPI.Services;

public class UserRepository(PethouseDbContext context) : IRepository<User>
{
    public IQueryable<User> GetAll()
    {
        return context.Users.AsNoTracking();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var result = await context.Users.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException("Id does not exist");
        
        return result;
    }

    public async Task AddAsync(User entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
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
        context.Users.Remove(result);
        await context.SaveChangesAsync();
    }
}