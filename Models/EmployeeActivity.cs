using System;

namespace PunchApiProject.Models
{
    public class EmployeeActivity
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public DateTime LastLogin { get; set; }
        public int TotalPunches { get; set; }

        // 🔹 Add these so your service compiles
        public DateTime PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
    }
}