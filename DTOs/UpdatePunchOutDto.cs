namespace PunchApiProject.DTOs
{
    public class UpdatePunchOutDto
    {
        public int EmployeeId { get; set; } = 0;

        public DateTime PunchOutTime { get; set; } = DateTime.MinValue;

    }
}
