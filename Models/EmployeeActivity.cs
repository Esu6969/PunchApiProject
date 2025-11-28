using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    [Table("EmployeeActivities")]
    public class EmployeeActivity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime PunchInTime { get; set; }

        public DateTime? PunchOutTime { get; set; }
    }
}