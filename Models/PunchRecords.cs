namespace PunchApiProject.Models
{
    public class PunchRecord
    {
        public int PunchId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime PunchTime { get; set; }
        public string PunchType { get; set; } = string.Empty;  // "In" or "Out"
    }
}
