using PunchApiProject.Models;
using PunchApiProject.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync();
        Task<IEnumerable<PunchRecord>> GetPunchRecordsByEmployeeIdAsync(int employeeId);
        Task<PunchRecord?> GetPunchRecordByIdAsync(int id);
        Task<ApiResponse> PunchInAsync(int employeeId);
        Task<ApiResponse> PunchOutAsync(int employeeId);
        Task<object> GetEmployeeStatsAsync(int employeeId);
    }
}