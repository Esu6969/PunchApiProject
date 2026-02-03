namespace PunchApiProject.DTOs
{
    public class RegisterDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? Position { get; set; } = string.Empty;
    }
}
