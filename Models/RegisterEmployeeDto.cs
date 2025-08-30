namespace PunchApiProject.Models
{
    public class RegisterEmployeeDto
    {
        public int? EmployeeId { get; set; } // Optional, can be null if auto-generated
        public string EmployeeName { get; set; }
        public string Email { get; set; }
    }
}
