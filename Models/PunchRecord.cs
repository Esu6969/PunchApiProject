using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    public class PunchRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }
        public Employee Employee { get; set; }
    }
}
