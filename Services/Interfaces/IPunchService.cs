using PunchApiProject.DTOs;
using PunchApiProject.Models;

namespace PunchApiProject.Services.Interfaces
{
    public interface IPunchService
    {
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync();
        Task<IEnumerable<PunchRecord>> GetPunchRecordsByEmployeeIdAsync(int employeeId);
        Task<PunchRecord?> GetPunchRecordByIdAsync(int id);
        Task<ApiResponse> PunchInAsync(int employeeId);
        Task<ApiResponse> PunchOutAsync(int employeeId);
        Task<object> GetEmployeeStatsAsync(int employeeId);
        Task<IEnumerable<PunchRecord>> GetRecordsByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}