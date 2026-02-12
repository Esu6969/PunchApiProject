namespace PunchApiProject.Models
{
    public class RegisterEmployee
    {
        public int? EmployeeId { get; set; } // Optional, can be null if auto-generated
        public string EmployeeName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public IFormFile? ProfilePicture { get; set; } // Optional profile picture
    }
}
