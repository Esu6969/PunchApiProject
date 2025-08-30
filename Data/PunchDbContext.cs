using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class PunchDbContext : DbContext
    {
        public PunchDbContext(DbContextOptions<PunchDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Username)
                .IsUnique();
        }

        public DbSet<Punch> Punches { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }
    }
}
