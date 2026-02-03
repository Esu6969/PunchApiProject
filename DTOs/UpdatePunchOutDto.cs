namespace PunchApiProject.DTOs
{
    public class UpdatePunchOutDto
    {
        public int EmployeeId { get; set; } = 0;  // Or use int? if it's optional
        public DateTime PunchOutTime { get; set; } = DateTime.MinValue;  // Or use DateTime? if it's optional
    }
}
