using Microsoft.EntityFrameworkCore;
using PethouseAPI.Entities;

namespace PethouseAPI.Data
{
    public class PethouseDbContext(DbContextOptions<PethouseDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
