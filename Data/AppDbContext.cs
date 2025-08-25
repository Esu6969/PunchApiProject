using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeActivity> EmployeeActivities { get; set; }
        public DbSet<Punch> Punches { get; set; }              // ✅ Add this
        public DbSet<LoginRecord> LoginRecords { get; set; }   // ✅ Add this if you use LoginRecord
    }
}