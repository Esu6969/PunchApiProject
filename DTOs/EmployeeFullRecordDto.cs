namespace PunchApiProject.DTOs
{
    public class EmployeeFullRecordDtos
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public List<DateTime> LoginTimes { get; set; } = new();
        public List<PunchDtos> PunchRecords { get; set; } = new();
    }

}
