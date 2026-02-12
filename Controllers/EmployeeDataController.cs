using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using PunchInOutContext;
using System.Security.Cryptography;
using System.Text;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeDataController : ControllerBase
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeDataController(PunchDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var employees = await _context.Employees
            //    .AsNoTracking()
            //    .Select(e => new
            //    {
            //        e.Id,
            //        e.EmployeeId,
            //        e.FirstName,
            //        e.LastName,
            //        e.Email,
            //        e.Phone,
            //        e.Department,
            //        e.Position,
            //        e.JoinDate,
            //        e.IsActive,
            //        e.CreatedAt,
            //        e.UpdatedAt
            //    })
            //    .ToListAsync();

            //return Ok(employees);

            
            using (PunchInOutDataContext db = new PunchInOutDataContext())
            {
                var allEmps = (from i in db.Employee1s
                               select new GetEmployeeDTO()
                               {
                                   EmployeeId = i.Id,
                                   EmployeeName = i.FirstName + " " + i.LastName,
                                   Department = i.Department,
                                   PhoneNo = i.Phone,
                                   Email = i.Email
                               }).ToList();

                return Ok(allEmps);
            }
        }

        /// <summary>
        /// Get employee by integer id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Phone,
                    e.Department,
                    e.Position,
                    e.JoinDate,
                    e.IsActive,
                    e.CreatedAt,
                    e.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound(new { success = false, message = "Employee not found" });
            }

            return Ok(employee);
        }

        /// <summary>
        /// Get employee by EmployeeId string.
        /// </summary>
        [HttpGet("by-employeeid/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return BadRequest(new { success = false, message = "EmployeeId is required" });
            }

            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => new
                {
                    e.Id,
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Phone,
                    e.Department,
                    e.Position,
                    e.JoinDate,
                    e.IsActive,
                    e.CreatedAt,
                    e.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound(new { success = false, message = "Employee not found" });
            }

            return Ok(employee);


        }

        /// <summary>
        /// Create a new employee.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { success = false, message = "Invalid payload" });
            }

            if (string.IsNullOrWhiteSpace(dto.EmployeeId) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { success = false, message = "EmployeeId, Email and Password are required" });
            }

            // Unique checks
            if (await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId))
            {
                return BadRequest(new { success = false, message = "EmployeeId already exists" });
            }

            if (await _context.Employees.AnyAsync(e => e.Email == dto.Email))
            {
                return BadRequest(new { success = false, message = "Email already registered" });
            }

            var employee = new Models.Employee
            {
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName ?? string.Empty,
                LastName = dto.LastName ?? string.Empty,
                Email = dto.Email,
                Phone = dto.Phone ?? string.Empty,
                Department = dto.Department ?? string.Empty,
                Position = dto.Position ?? string.Empty,
                PasswordHash = HashPassword(dto.Password),
                JoinDate = dto.JoinDate ?? DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, new
            {
                success = true,
                message = "Employee created",
                employee.Id,
                employee.EmployeeId
            });
        }

        /// <summary>
        /// Update an existing employee.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { success = false, message = "Invalid payload" });
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound(new { success = false, message = "Employee not found" });
            }

            // Prevent changing to an already used EmployeeId or Email
            if (!string.IsNullOrWhiteSpace(dto.EmployeeId) && dto.EmployeeId != employee.EmployeeId)
            {
                if (await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId && e.Id != id))
                {
                    return BadRequest(new { success = false, message = "EmployeeId already in use" });
                }
                employee.EmployeeId = dto.EmployeeId;
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != employee.Email)
            {
                if (await _context.Employees.AnyAsync(e => e.Email == dto.Email && e.Id != id))
                {
                    return BadRequest(new { success = false, message = "Email already in use" });
                }
                employee.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.FirstName)) employee.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName)) employee.LastName = dto.LastName;
            if (dto.Phone != null) employee.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Department)) employee.Department = dto.Department;
            if (!string.IsNullOrWhiteSpace(dto.Position)) employee.Position = dto.Position;
            if (dto.JoinDate.HasValue) employee.JoinDate = dto.JoinDate.Value;
            if (dto.IsActive.HasValue) employee.IsActive = dto.IsActive.Value;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                employee.PasswordHash = HashPassword(dto.Password);
            }

            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Employee updated" });
        }

        /// <summary>
        /// Soft-delete (deactivate) an employee.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound(new { success = false, message = "Employee not found" });
            }

            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Employee deactivated" });
        }

        // Helper: simple SHA256 password hashing (same approach as AuthController)
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // DTOs for create/update
        public class EmployeeCreateDto
        {
            public string EmployeeId { get; set; } = string.Empty;
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string Email { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string? Department { get; set; }
            public string? Position { get; set; }
            public DateTime? JoinDate { get; set; }
            public string Password { get; set; } = string.Empty;
        }

        public class EmployeeUpdateDto
        {
            public string? EmployeeId { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Department { get; set; }
            public string? Position { get; set; }
            public DateTime? JoinDate { get; set; }
            public bool? IsActive { get; set; }
            public string? Password { get; set; }
        }
    }
}