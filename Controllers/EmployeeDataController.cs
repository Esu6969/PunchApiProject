using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using System.Security.Cryptography;
using System.Text;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeDataController : ControllerBase
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<EmployeeDataController> _logger;

        public EmployeeDataController(PunchDbContext context, ILogger<EmployeeDataController> logger)
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
            try
            {
                var employees = await _context.Employees
                    .AsNoTracking()
                    .Where(e => e.IsActive)
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
                    .ToListAsync();

                return Ok(new { success = true, data = employees });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return StatusCode(500, new { success = false, message = "Error retrieving employees", error = ex.Message });
            }
        }

        /// <summary>
        /// Get employee by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

                if (employee == null)
                    return NotFound(new { success = false, message = "Employee not found" });

                return Ok(new { success = true, data = employee });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee {Id}", id);
                return StatusCode(500, new { success = false, message = "Error retrieving employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Update employee.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return NotFound(new { success = false, message = "Employee not found" });

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

                return Ok(new { success = true, message = "Employee updated successfully", data = employee });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee {Id}", id);
                return StatusCode(500, new { success = false, message = "Error updating employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete employee (soft delete).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return NotFound(new { success = false, message = "Employee not found" });

                employee.IsActive = false;
                employee.UpdatedAt = DateTime.UtcNow;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee {Id}", id);
                return StatusCode(500, new { success = false, message = "Error deleting employee", error = ex.Message });
            }
        }
    }
}