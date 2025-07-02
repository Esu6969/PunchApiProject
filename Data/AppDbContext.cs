using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PunchRecord> PunchRecords { get; set; }
    }
}
