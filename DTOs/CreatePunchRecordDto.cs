namespace PunchApiProject.DTOs
{
    public class CreatePunchRecordDto
    {
        public int EmployeeId { get; set; }
        public string ActionType { get; set; } = string.Empty; // "PunchIn" or "PunchOut"
    }
}
