using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(PunchDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto dto)
        {
            try
            {
                _logger.LogInformation("Registration attempt for Employee ID: {EmployeeId}", dto.EmployeeId);

                // Validate input
                if (string.IsNullOrEmpty(dto.EmployeeId) || string.IsNullOrEmpty(dto.Email))
                {
                    return BadRequest(new { success = false, message = "Employee ID and Email are required" });
                }

                // Check if employee ID already exists
                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId);

                if (existingEmployee != null)
                {
                    return BadRequest(new { success = false, message = "Employee ID already exists" });
                }

                // Check if email already exists
                var existingEmail = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == dto.Email);

                if (existingEmail != null)
                {
                    return BadRequest(new { success = false, message = "Email already registered" });
                }

                // Create new employee
                var employee = new Employee
                {
                    EmployeeId = dto.EmployeeId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone ?? "",
                    Department = dto.Department,
                    Position = dto.Position,
                    PasswordHash = HashPassword(dto.Password),
                    JoinDate = string.IsNullOrEmpty(dto.JoinDate) 
                        ? DateTime.UtcNow 
                        : DateTime.Parse(dto.JoinDate),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

            //for loop (phone contact)
                foreach (var contact in dto.Phones)
                {
                    var ec = new EmployeeContacts
                    {
                        EmployeeId = employee.Id,
                        ContactNumber = contact
                    };
                    _context.EmployeeContacts.Add(ec);
                    _context.SaveChanges();
                }


                _logger.LogInformation("Employee registered successfully: {EmployeeId}", employee.EmployeeId);

                return Ok(new
                {
                    success = true,
                    message = "Employee registered successfully",
                    employeeId = employee.EmployeeId,
                    name = $"{employee.FirstName} {employee.LastName}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for Employee ID: {EmployeeId}", dto?.EmployeeId);
                return StatusCode(500, new 
                { 
                    success = false,
                    message = "Registration failed. Please try again.",
                    error = ex.Message 
                });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                _logger.LogInformation("Login attempt for Employee ID: {EmployeeId}", dto.EmployeeId);

                if (string.IsNullOrEmpty(dto.EmployeeId))
                {
                    return BadRequest(new { success = false, message = "Employee ID is required" });
                }

                // Find employee by EmployeeId STRING field (not the Id INT field)
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId && e.IsActive);

                if (employee == null)
                {
                    return Unauthorized(new { success = false, message = "Invalid Employee ID or account is inactive" });
                }

                _logger.LogInformation("Employee found: {EmployeeId} - {Name}", employee.EmployeeId, $"{employee.FirstName} {employee.LastName}");

                // TODO: Add password verification here
                // For now, just return employee data
                return Ok(new
                {
                    success = true,
                    employeeId = employee.EmployeeId,
                    id = employee.Id, // Include integer ID for punch operations
                    name = $"{employee.FirstName} {employee.LastName}",
                    email = employee.Email,
                    department = employee.Department,
                    position = employee.Position
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for Employee ID: {EmployeeId}", dto?.EmployeeId);
                return StatusCode(500, new 
                { 
                    success = false,
                    message = "Login failed. Please try again.",
                    error = ex.Message 
                });
            }
        }

        // Simple password hashing method
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    // DTOs (Data Transfer Objects)
    public class EmployeeRegistrationDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string? JoinDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public List<string> Phones { get; set;  } = new List<string>();
    }

    public class LoginDto
    {
        public string EmployeeId { get; set; } = string.Empty;
    }
}