using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }
        public DbSet<PunchRecord> PunchRecords { get; set; }
    }
}