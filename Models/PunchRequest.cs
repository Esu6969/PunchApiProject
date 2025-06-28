namespace PunchApiProject.Models
{
    public class PunchRequest
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string PunchType { get; set; } = string.Empty;  // "In" or "Out"
    }
}
