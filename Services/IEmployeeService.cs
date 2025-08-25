using PunchApiProject.Models;
using PunchApiProject.DTOs;

namespace PunchApiProject.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<ServiceResponse<string>> RegisterAsync(RegisterDto request);
        Task<IEnumerable<EmployeeActivity>> GetAllEmployeeActivityAsync();
    }
}