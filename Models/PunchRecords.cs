namespace PunchApiProject.Models
{
    public class PunchRecord
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }
    }
}
