using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using PunchApiProject.Services.Interfaces;

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

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.EmployeeId)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.PunchRecords)
                .Include(e => e.EmployeeContacts)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetEmployeeByEmployeeIdAsync(string employeeId)
        {
            return await _context.Employees
                .Include(e => e.PunchRecords)
                .Include(e => e.EmployeeContacts)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New employee added: {EmployeeId} ({Name})", employee.EmployeeId, employee.FullName);
            return employee;
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return null;

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

            _logger.LogInformation("Employee updated: {EmployeeId}", employee.EmployeeId);
            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
                return false;

            emp.IsActive = false;
            emp.UpdatedAt = DateTime.UtcNow;
            _context.Employees.Update(emp);
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(emp.Id, "Delete", "Employee account deactivated");

            _logger.LogInformation("Employee deleted: {EmployeeId}", emp.EmployeeId);
            return true;
        }

        public async Task<IEnumerable<EmployeeActivity>> GetAllEmployeeActivityAsync()
        {
            return await _context.EmployeeActivities.ToListAsync();
        }

        public async Task<ApiResponse> RegisterAsync(EmployeeRegisterDto request)
        {
            var response = new ApiResponse();

            try
            {
                if (string.IsNullOrEmpty(request.EmployeeId) || string.IsNullOrEmpty(request.Password))
                {
                    response.Success = false;
                    response.Message = "Employee ID and password are required";
                    return response;
                }

                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId);

                if (existingEmployee != null)
                {
                    response.Success = false;
                    response.Message = "Employee ID already exists";
                    return response;
                }

                response.Success = true;
                response.Message = "Registration validated successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration validation error");
                response.Success = false;
                response.Message = "Registration validation failed";
                response.Errors.Add(ex.Message);
            }

            return response;
        }
    }
}