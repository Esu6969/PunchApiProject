namespace PunchApiProject.Models
{
    public class Punch
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
    }
}
