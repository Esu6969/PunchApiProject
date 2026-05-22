using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using PunchApiProject.Services.Interfaces;
using BCrypt.Net;

namespace PunchApiProject.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IAuditService _auditService;

        public EmployeeService(PunchDbContext context, ILogger<EmployeeService> logger, IAuditService auditService)
        {
            _context = context;
            _logger = logger;
            _auditService = auditService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        public async Task<ApiResponse<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            var response = new ApiResponse<IEnumerable<Employee>>();

            try
            {
                var employees = await _context.Employees
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.EmployeeId)
                    .ToListAsync();

                response.Success = true;
                response.Message = $"Retrieved {employees.Count} employees";
                response.Data = employees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all employees");
                response.Success = false;
                response.Message = "Failed to retrieve employees";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get employee by database ID
        /// </summary>
        public async Task<ApiResponse<Employee>> GetEmployeeByIdAsync(int id)
        {
            var response = new ApiResponse<Employee>();

            try
            {
                var employee = await _context.Employees
                    .Include(e => e.PunchRecords)
                    .Include(e => e.EmployeeContacts)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                response.Success = true;
                response.Data = employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee {EmployeeId}", id);
                response.Success = false;
                response.Message = "Failed to retrieve employee";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get employee by Employee ID (string)
        /// </summary>
        public async Task<ApiResponse<Employee>> GetEmployeeByEmployeeIdAsync(string employeeId)
        {
            var response = new ApiResponse<Employee>();

            try
            {
                var employee = await _context.Employees
                    .Include(e => e.PunchRecords)
                    .Include(e => e.EmployeeContacts)
                    .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                response.Success = true;
                response.Data = employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee {EmployeeId}", employeeId);
                response.Success = false;
                response.Message = "Failed to retrieve employee";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Add new employee
        /// </summary>
        public async Task<ApiResponse<Employee>> AddEmployeeAsync(Employee employee)
        {
            var response = new ApiResponse<Employee>();

            try
            {
                // Check duplicates
                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

                if (existingEmployee != null)
                {
                    response.Success = false;
                    response.Message = "Employee ID already exists";
                    return response;
                }

                var existingEmail = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == employee.Email);

                if (existingEmail != null)
                {
                    response.Success = false;
                    response.Message = "Email already registered";
                    return response;
                }

                employee.CreatedAt = DateTime.UtcNow;
                employee.UpdatedAt = DateTime.UtcNow;

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = $"Employee {employee.FullName} added successfully";
                response.Data = employee;

                _logger.LogInformation("New employee added: {EmployeeId} ({Name})", employee.EmployeeId, employee.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee");
                response.Success = false;
                response.Message = "Failed to add employee";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Update employee
        /// </summary>
        public async Task<ApiResponse<Employee>> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
        {
            var response = new ApiResponse<Employee>();

            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                if (!string.IsNullOrEmpty(dto.FirstName))
                    employee.FirstName = dto.FirstName;

                if (!string.IsNullOrEmpty(dto.LastName))
                    employee.LastName = dto.LastName;

                if (!string.IsNullOrEmpty(dto.Phone))
                    employee.Phone = dto.Phone;

                if (!string.IsNullOrEmpty(dto.Department))
                    employee.Department = dto.Department;

                if (!string.IsNullOrEmpty(dto.Position))
                    employee.Position = dto.Position;

                if (dto.IsActive.HasValue)
                    employee.IsActive = dto.IsActive.Value;

                employee.UpdatedAt = DateTime.UtcNow;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                await _auditService.LogActionAsync(employee.Id, "Update", "Employee information updated");

                response.Success = true;
                response.Message = "Employee updated successfully";
                response.Data = employee;

                _logger.LogInformation("Employee updated: {EmployeeId}", employee.EmployeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee {EmployeeId}", id);
                response.Success = false;
                response.Message = "Failed to update employee";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Delete employee (soft delete)
        /// </summary>
        public async Task<ApiResponse> DeleteEmployeeAsync(int id)
        {
            var response = new ApiResponse();

            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                employee.IsActive = false;
                employee.UpdatedAt = DateTime.UtcNow;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                await _auditService.LogActionAsync(employee.Id, "Delete", "Employee account deactivated");

                response.Success = true;
                response.Message = $"Employee {employee.FullName} deleted successfully";

                _logger.LogInformation("Employee deleted: {EmployeeId}", employee.EmployeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee {EmployeeId}", id);
                response.Success = false;
                response.Message = "Failed to delete employee";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Change employee password
        /// </summary>
        public async Task<ApiResponse> ChangePasswordAsync(int employeeId, string oldPassword, string newPassword)
        {
            var response = new ApiResponse();

            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                // Verify old password
                if (!BCrypt.Net.BCrypt.Verify(oldPassword, employee.PasswordHash))
                {
                    response.Success = false;
                    response.Message = "Old password is incorrect";
                    return response;
                }

                // Hash new password
                employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                employee.UpdatedAt = DateTime.UtcNow;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                await _auditService.LogActionAsync(employeeId, "PasswordChange", "Employee password changed");

                response.Success = true;
                response.Message = "Password changed successfully";

                _logger.LogInformation("Password changed for employee {EmployeeId}", employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for employee {EmployeeId}", employeeId);
                response.Success = false;
                response.Message = "Failed to change password";
                response.Errors.Add(ex.Message);
            }

            return response;
        }
    }
}