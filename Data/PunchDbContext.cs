using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class PunchDbContext : DbContext
    {
        public PunchDbContext(DbContextOptions<PunchDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<PunchRecord> PunchRecords { get; set; }
    }
}
