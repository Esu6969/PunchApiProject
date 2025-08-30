using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using PunchApiProject.Models;
using System;
using System.Threading.Tasks;
using PunchApiProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        private readonly PunchDbContext _context;

        public PunchController(IPunchService punchService, PunchDbContext context)
        {
            _punchService = punchService;
            _context = context;
        }

        private string GenerateJwtToken(Employee employee)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere!")); // Use a secure key from config
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.EmployeeName) || string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest(new { message = "Invalid employee details." });

                // Check if username already exists
                var exists = await _context.Employees.AnyAsync(e => e.Username == dto.EmployeeName);
                if (exists)
                    return Conflict(new { message = "Employee with this name already exists. Please use a different name." });

                var employee = new Employee
                {
                    Username = dto.EmployeeName,
                    PasswordHash = new byte[0],
                    PasswordSalt = new byte[0]
                };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                var punch = new Punch
                {
                    EmployeeId = employee.Id,
                    EmployeeName = dto.EmployeeName,
                    ActionDateTime = DateTime.Now,
                    ActionType = "Registration"
                };

                try
                {
                    await _punchService.AddPunchRecordAsync(punch);
                }
                catch (Exception ex)
                {
                    var inner = ex.InnerException?.Message ?? "";
                    return StatusCode(500, new { message = "Error saving Punch record", error = ex.Message, innerException = inner, stack = ex.StackTrace, punch });
                }

                var token = "dummy-token"; // Replace with JWT logic if needed

                return Ok(new
                {
                    message = "Your registration is successfully completed.",
                    token,
                    punch
                });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "";
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message, innerException = inner, stack = ex.StackTrace });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == dto.EmployeeName);
            if (employee == null)
                return Unauthorized(new { message = "Invalid credentials." });

            // Add login record
            var login = new LoginRecord
            {
                EmployeeId = employee.Id,
                LoginTime = DateTime.Now
            };
            _context.LoginRecords.Add(login);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(employee);

            return Ok(new
            {
                message = "You're login successfully.",
                token
            });
        }

        [HttpPost("in")]
        public async Task<IActionResult> PunchIn([FromHeader(Name = "Authorization")] string token, [FromBody] Punch punch)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Token == token);
            if (employee == null)
                return Unauthorized(new { message = "Invalid or missing token." });

            if (punch == null || punch.EmployeeId != employee.Id || string.IsNullOrWhiteSpace(punch.EmployeeName))
                return BadRequest(new { message = "Invalid employee details." });

            punch.ActionDateTime = DateTime.Now;
            punch.ActionType = "PunchIn";
            await _punchService.AddPunchRecordAsync(punch);

            return Ok(new
            {
                message = "Punch in successfully.",
                date = punch.ActionDateTime.ToString("yyyy-MM-dd"),
                time = punch.ActionDateTime.ToString("HH:mm:ss"),
                day = punch.ActionDateTime.DayOfWeek.ToString()
            });
        }

        [HttpPost("out")]
        public async Task<IActionResult> PunchOut([FromHeader(Name = "Authorization")] string token, [FromBody] Punch punch)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Token == token);
            if (employee == null)
                return Unauthorized(new { message = "Invalid or missing token." });

            if (punch == null || punch.EmployeeId != employee.Id || string.IsNullOrWhiteSpace(punch.EmployeeName))
                return BadRequest(new { message = "Invalid employee details." });

            punch.ActionDateTime = DateTime.Now;
            punch.ActionType = "PunchOut";
            await _punchService.AddPunchRecordAsync(punch);

            var totalHours = await _punchService.CalculateTotalHoursAsync(punch.EmployeeId);

            return Ok(new
            {
                message = "Punch out successfully.",
                totalHoursFormatted = $"{(int)totalHours.TotalHours} hours {totalHours.Minutes} minutes",
                date = punch.ActionDateTime.ToString("yyyy-MM-dd"),
                time = punch.ActionDateTime.ToString("HH:mm:ss"),
                day = punch.ActionDateTime.DayOfWeek.ToString()
            });
        }

        [HttpGet("totalhours/{id}")]
        public async Task<IActionResult> GetTotalHours(int id, [FromHeader(Name = "Authorization")] string token)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Token == token && e.Id == id);
            if (employee == null)
                return Unauthorized(new { message = "Invalid or missing token." });

            var totalHours = await _punchService.CalculateTotalHoursAsync(id);

            return Ok(new
            {
                employeeId = id,
                totalHoursFormatted = $"{(int)totalHours.TotalHours} hours {totalHours.Minutes} minutes",
                totalHoursRaw = totalHours
            });
        }
    }
}