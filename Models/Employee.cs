using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Full name property for convenience
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation property
        public virtual ICollection<PunchRecord> PunchRecords { get; set; } = new List<PunchRecord>();
    }
}