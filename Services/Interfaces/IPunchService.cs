using PunchApiProject.DTOs;
using PunchApiProject.Models;

namespace PunchApiProject.Services.Interfaces
{
    public interface IPunchService
    {
        Task<ApiResponse<IEnumerable<PunchRecordDto>>> GetAllPunchRecordsAsync();
        Task<ApiResponse<IEnumerable<PunchRecordDto>>> GetPunchRecordsByEmployeeIdAsync(int employeeId);
        Task<ApiResponse<PunchRecordDto>> GetPunchRecordByIdAsync(int id);
        Task<ApiResponse<object>> PunchInAsync(int employeeId);
        Task<ApiResponse<object>> PunchOutAsync(int employeeId);
        Task<ApiResponse<EmployeeStatsDto>> GetEmployeeStatsAsync(int employeeId);
        Task<ApiResponse<IEnumerable<PunchRecordDto>>> GetRecordsByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}