namespace PunchApiProject.Services.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateMonthlyReportAsync(int employeeId, int month, int year);
        Task<byte[]> GenerateDepartmentReportAsync(string department, DateTime startDate, DateTime endDate);
        Task<object> GetEmployeeAttendanceAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}