using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    public class PunchRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime ActionDateTime { get; set; }
        public string ActionType { get; set; } = string.Empty; // "PunchIn" or "PunchOut"
    }
}
