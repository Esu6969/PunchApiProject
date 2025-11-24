using PunchApiProject.Models;
using PunchApiProject.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<IEnumerable<EmployeeActivity>> GetAllEmployeeActivityAsync();
        Task<ApiResponse> RegisterAsync(EmployeeRegisterDto request);
    }
}