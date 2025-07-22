using Microsoft.EntityFrameworkCore;
using SigortaAPI.Models;

namespace SigortaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablo setleri:
        public DbSet<User> Users { get; set; }
        // İleride ekleyeceğim:
        // public DbSet<Policy> Policies { get; set; }
        // public DbSet<Claim> Claims { get; set; }
    }
}
