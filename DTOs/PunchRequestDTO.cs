namespace PunchApiProject.DTOs
{
    public class PunchRequestDTO
    {
        public string EmployeeName { get; set; } = string.Empty; // ✅ Safe default
    }
}
