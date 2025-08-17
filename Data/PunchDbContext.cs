using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class PunchDbContext : DbContext
    {
        public PunchDbContext(DbContextOptions<PunchDbContext> options) : base(options) { }

        public DbSet<Punch> Punches { get; set; }
    }
}
