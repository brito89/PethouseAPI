using PethouseAPI.Data;
using PethouseAPI.Entities;

namespace PethouseAPI.Services;

public class BreedSizeRepository(PethouseDbContext context) : IRepository<BreedSize>
{
    public IQueryable<BreedSize> GetAll()
    {
        return context.BreedSizes.AsQueryable();
    }

    public async Task<BreedSize> GetByIdAsync(int id)
    {
        var breedSize = await context.BreedSizes.FindAsync(id);
        
        if (breedSize is null)
            throw new KeyNotFoundException("Id does not exist");
        
        return breedSize;
    }

    public async Task AddAsync(BreedSize entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        var result = await context.BreedSizes.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BreedSize entity)
    {
        var breeedsize = await GetByIdAsync(entity.Id);
        
        if(breeedsize is null)
            throw new ArgumentNullException(nameof(entity));
        
        var result = context.BreedSizes.Update(entity);
        await context.SaveChangesAsync();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}