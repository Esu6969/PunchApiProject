using System;
using System.ComponentModel.DataAnnotations;

namespace PunchApiProject.Models
{
    public class EmployeeActivity
    {
        [Key] // ✅ Mark this as primary key
        public int ActivityId { get; set; }

        public int EmployeeId { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }
    }
}