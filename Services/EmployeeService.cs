using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Models;
using PunchApiProject.DTOs;  // ← CHANGED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return false;

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
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
                // Validate input
                if (string.IsNullOrEmpty(request.EmployeeId) || string.IsNullOrEmpty(request.Password))
                {
                    response.Success = false;
                    response.Message = "Employee ID and password are required";
                    return response;
                }

                // Check if employee already exists
                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId);

                if (existingEmployee != null)
                {
                    response.Success = false;
                    response.Message = "Employee ID already exists";
                    return response;
                }

                // Check if email already exists
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var existingEmail = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Email == request.Email);

                    if (existingEmail != null)
                    {
                        response.Success = false;
                        response.Message = "Email already registered";
                        return response;
                    }
                }

                // Create new employee
                var employee = new Employee
                {
                    EmployeeId = request.EmployeeId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone ?? "",
                    Department = request.Department,
                    Position = request.Position,
                    PasswordHash = HashPassword(request.Password),
                    JoinDate = string.IsNullOrEmpty(request.JoinDate) 
                        ? DateTime.UtcNow 
                        : DateTime.Parse(request.JoinDate),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = employee.EmployeeId;
                response.Message = $"Employee {employee.FirstName} {employee.LastName} registered successfully!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Registration failed: {ex.Message}";
            }

            return response;
        }

        // Simple password hashing method
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}