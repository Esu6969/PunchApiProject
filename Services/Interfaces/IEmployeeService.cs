using PunchApiProject.DTOs;
using PunchApiProject.Models;

namespace PunchApiProject.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse<IEnumerable<Employee>>> GetAllEmployeesAsync();
        Task<ApiResponse<Employee>> GetEmployeeByIdAsync(int id);
        Task<ApiResponse<Employee>> AddEmployeeAsync(Employee employee);
        Task<ApiResponse<Employee>> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto);
        Task<ApiResponse> DeleteEmployeeAsync(int id);
        Task<ApiResponse<Employee>> GetEmployeeByEmployeeIdAsync(string employeeId);
        Task<ApiResponse> ChangePasswordAsync(int employeeId, string oldPassword, string newPassword);
    }
}