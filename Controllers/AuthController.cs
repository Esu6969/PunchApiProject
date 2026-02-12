// Controllers/AuthController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Models;
using PunchApiProject.Middleware; // ✅ import session extensions

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

        // ── POST: api/auth/register ──────────────────────────────
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto dto)
        {
            try
            {
                _logger.LogInformation("Registration attempt: {EmployeeId}", dto.EmployeeId);

                if (string.IsNullOrEmpty(dto.EmployeeId) || string.IsNullOrEmpty(dto.Email))
                    return BadRequest(new { success = false, message = "Employee ID and Email are required" });

                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId);
                if (existingEmployee != null)
                    return BadRequest(new { success = false, message = "Employee ID already exists" });

                var existingEmail = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == dto.Email);
                if (existingEmail != null)
                    return BadRequest(new { success = false, message = "Email already registered" });

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

                // ✅ Save all phone contacts in one go (not inside loop)
                foreach (var contact in dto.Phones)
                {
                    _context.EmployeeContacts.Add(new EmployeeContacts
                    {
                        EmployeeId = employee.Id,
                        ContactNumber = contact
                    });
                }
                await _context.SaveChangesAsync(); // single save for all contacts

                _logger.LogInformation("Registered successfully: {EmployeeId}", employee.EmployeeId);

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
                _logger.LogError(ex, "Registration failed: {EmployeeId}", dto?.EmployeeId);
                return StatusCode(500, new { success = false, message = "Registration failed", error = ex.Message });
            }
        }

        // ── POST: api/auth/login ─────────────────────────────────
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                _logger.LogInformation("Login attempt: {EmployeeId}", dto.EmployeeId);

                if (string.IsNullOrEmpty(dto.EmployeeId))
                    return BadRequest(new { success = false, message = "Employee ID is required" });

                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId && e.IsActive);

                if (employee == null)
                    return Unauthorized(new { success = false, message = "Invalid Employee ID or inactive account" });

                // ✅ Set session using extension method (one clean line)
                HttpContext.SetLoginSession(
                    employeeId: employee.EmployeeId,
                    employeeName: $"{employee.FirstName} {employee.LastName}",
                    department: employee.Department,
                    position: employee.Position,
                    id: employee.Id
                );

                _logger.LogInformation("Login successful, session created: {EmployeeId}", employee.EmployeeId);

                return Ok(new
                {
                    success = true,
                    message = "Login successful",
                    employeeId = employee.EmployeeId,
                    id = employee.Id,
                    name = $"{employee.FirstName} {employee.LastName}",
                    email = employee.Email,
                    department = employee.Department,
                    position = employee.Position
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed: {EmployeeId}", dto?.EmployeeId);
                return StatusCode(500, new { success = false, message = "Login failed", error = ex.Message });
            }
        }

        // ── POST: api/auth/logout ────────────────────────────────
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var employeeId = HttpContext.GetSessionEmployeeId();

            // ✅ Clear session using extension method
            HttpContext.ClearLoginSession();

            _logger.LogInformation("Logged out: {EmployeeId}", employeeId);
            return Ok(new { success = true, message = "Logged out successfully" });
        }

        // ── GET: api/auth/me ─────────────────────────────────────
        // Check who is currently logged in
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            // ✅ Read session using extension methods
            var employeeId = HttpContext.GetSessionEmployeeId();

            if (string.IsNullOrEmpty(employeeId))
                return Unauthorized(new { success = false, message = "Not logged in" });

            return Ok(new
            {
                success = true,
                employeeId = employeeId,
                name = HttpContext.GetSessionEmployeeName(),
                department = HttpContext.GetSessionDepartment(),
                position = HttpContext.GetSessionPosition(),
                id = HttpContext.GetSessionId()
            });
        }

        // ── Private Helpers ──────────────────────────────────────
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    // ── DTOs ─────────────────────────────────────────────────────
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
        public List<string> Phones { get; set; } = new List<string>();
    }

    public class LoginDto
    {
        public string EmployeeId { get; set; } = string.Empty;
    }
}