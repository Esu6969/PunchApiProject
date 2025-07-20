using PunchApiProject.Models;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        Task PunchInAsync(int employeeId);
        Task PunchOutAsync(int employeeId);
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync();
        Task<IEnumerable<PunchRecord>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate);
        Task<TimeSpan> CalculateTotalHoursAsync(int employeeId);
    }
}
