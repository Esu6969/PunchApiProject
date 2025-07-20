namespace PunchApiProject.Models
{
    public class LoginRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;

        public Employee Employee { get; set; }
    }
}