using PunchApiProject.DTOs;
using PunchApiProject.Models;

namespace PunchApiProject.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeByEmployeeIdAsync(string employeeId);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<IEnumerable<EmployeeActivity>> GetAllEmployeeActivityAsync();
        Task<ApiResponse> RegisterAsync(EmployeeRegisterDto request);
    }
}