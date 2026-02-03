namespace PunchApiProject.Models
{
    public class LoginDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // If you want to use passwords in the future
    }
}
