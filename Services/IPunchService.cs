using PunchApiProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        Task<PunchRecord> AddPunchRecordAsync(int employeeId, string actionType);
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync();
        Task<Employee> RegisterEmployeeAsync(string name, string email);
    }
}