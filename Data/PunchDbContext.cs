using Microsoft.EntityFrameworkCore;
using PunchApiProject.Models;

namespace PunchApiProject.Data
{
    public class PunchDbContext : DbContext
    {
        public PunchDbContext(DbContextOptions<PunchDbContext> options) : base(options) 
        { 
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<PunchRecord> PunchRecords { get; set; } = null!;
        public DbSet<EmployeeActivity> EmployeeActivities { get; set; } = null!;
        public DbSet<EmployeeContacts> EmployeeContacts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee table with proper column names
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees"); // Lowercase table name for PostgreSQL
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
                entity.Property(e => e.Department).HasColumnName("department").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Position).HasColumnName("position").IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);
                entity.Property(e => e.JoinDate).HasColumnName("join_date").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.EmployeeId).IsUnique();
            });

            // Configure PunchRecord table
            modelBuilder.Entity<PunchRecord>(entity =>
            {
                entity.ToTable("punch_records");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(p => p.EmployeeId).HasColumnName("employee_id").IsRequired();
                entity.Property(p => p.ActionDateTime).HasColumnName("action_date_time").IsRequired();
                entity.Property(p => p.ActionType).HasColumnName("action_type").IsRequired().HasMaxLength(20);
                
                entity.HasOne(p => p.Employee)
                      .WithMany(e => e.PunchRecords)
                      .HasForeignKey(p => p.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure EmployeeActivity table
            modelBuilder.Entity<EmployeeActivity>(entity =>
            {
                entity.ToTable("employee_activities");
                entity.HasKey(e => e.ActivityId);
                entity.Property(e => e.ActivityId).HasColumnName("activity_id").ValueGeneratedOnAdd();
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id").IsRequired();
                entity.Property(e => e.PunchInTime).HasColumnName("punch_in_time").IsRequired();
                entity.Property(e => e.PunchOutTime).HasColumnName("punch_out_time");
            });

            // Configure EmployeeContact table
            modelBuilder.Entity<EmployeeContacts>(entity =>
            {
                entity.ToTable("employee_contacts");
                entity.HasKey(e => e.EmployeeContactId);

                entity.HasOne(p => p.Employee)
                     .WithMany(e => e.EmployeeContacts)
                     .HasForeignKey(p => p.EmployeeId)
                     .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}