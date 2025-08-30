using System;

namespace PunchApiProject.Models
{
    public class Punch
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } // New: Employee name
        public DateTime ActionDateTime { get; set; } // New: Date and time of action
        public string ActionType { get; set; } // New: "Registration", "PunchIn", "PunchOut"
        public string Email { get; set; } // Add this if you want to store email

        // 🔹 Add navigation property so EF can link Punch -> Employee
        public Employee Employee { get; set; } 
    }
}