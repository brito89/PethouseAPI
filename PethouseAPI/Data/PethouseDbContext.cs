using Microsoft.EntityFrameworkCore;
using PethouseAPI.Entities;

namespace PethouseAPI.Data
{
    public class PethouseDbContext(DbContextOptions<PethouseDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BreedSize> BreedSizes { get; set; }

        public DbSet<PeakSeason> PeakSeasons { get; set; }
        
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        
        public DbSet<PetAppointment> PetAppointments { get; set; }

    }
}
