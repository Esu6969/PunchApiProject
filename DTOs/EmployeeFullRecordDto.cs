namespace PunchApiProject.DTOs
{
    public class EmployeeFullRecordDto
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public List<DateTime> LoginTimes { get; set; } = new();
        public List<PunchDto> PunchRecords { get; set; } = new();
    }

}
