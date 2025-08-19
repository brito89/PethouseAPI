using Microsoft.EntityFrameworkCore;
using PethouseAPI.Data;
using PethouseAPI.Entities;
using PethouseAPI.Services;

namespace PethouseTest;

public class BreedSizeRepositoryTests
{
    private PethouseDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PethouseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Isolated per test
            .Options;

        return new PethouseDbContext(options);
    }
    
    private BreedSizeRepository GetRepository(PethouseDbContext context)
    {
        return new BreedSizeRepository(context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var breedSize = new BreedSize { Id = 4, Name = "Grande" };

        await repo.AddAsync(breedSize);

        var result = await context.BreedSizes.FindAsync(4);
        Assert.NotNull(result);
        Assert.Equal("Grande", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);
        
        context.BreedSizes.Add(new BreedSize { Id = 2, Name = "Medium" });
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(2);
        Assert.NotNull(result);
        Assert.Equal("Medium",result.Name);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetByIdAsync(999));
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyEntity()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var original = new BreedSize { Id = 3, Name = "Large" };
        context.BreedSizes.Add(original);
        await context.SaveChangesAsync();

        var updated = new BreedSize { Id = 3, Name = "Extra Large" };
        await repo.UpdateAsync(updated);

        var result = await context.BreedSizes.FindAsync(3);
        Assert.Equal("Extra Large", result?.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        context.BreedSizes.Add(new BreedSize { Id = 4, Name = "Tiny" });
        await context.SaveChangesAsync();

        await repo.DeleteAsync(4);

        var result = await context.BreedSizes.FindAsync(4);
        Assert.Null(result);
    }


}