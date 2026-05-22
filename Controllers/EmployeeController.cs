// Controllers/EmployeeController.cs
// ✅ No manual session checks needed anywhere in this file
//    SessionMiddleware handles it automatically for all routes

using Microsoft.AspNetCore.Mvc;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using PunchApiProject.Services.Interfaces;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IReportService _reportService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IReportService reportService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Employee>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                // ✅ Optionally read who is making the request from session
                var callerName = HttpContext.GetSessionEmployeeName();
                _logger.LogInformation("GetAllEmployees called by: {Name}", callerName);

                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get employees");
                return StatusCode(500, new { message = "Failed to retrieve employees", error = ex.Message });
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse { Success = false, Message = "Valid ID is required" });

                var employee = await _employeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get employee {Id}", id);
                return StatusCode(500, new { message = "Failed to retrieve employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Get employee by Employee ID
        /// </summary>
        [HttpGet("by-id/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeByEmployeeId(string employeeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId))
                    return BadRequest(new ApiResponse { Success = false, Message = "Employee ID is required" });

                var employee = await _employeeService.GetEmployeeByEmployeeIdAsync(employeeId);

                if (employee == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get employee by Employee ID {EmployeeId}", employeeId);
                return StatusCode(500, new { message = "Failed to retrieve employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Add new employee
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Employee>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeRegistrationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors.ToList()
                    });
                }

                var employee = new Employee
                {
                    EmployeeId = dto.EmployeeId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone ?? "",
                    Department = dto.Department,
                    Position = dto.Position,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    JoinDate = string.IsNullOrEmpty(dto.JoinDate)
                        ? DateTime.UtcNow
                        : DateTime.Parse(dto.JoinDate),
                    IsActive = true
                };

                var newEmployee = await _employeeService.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create employee");
                return StatusCode(500, new { message = "Failed to create employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Update employee
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto dto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse { Success = false, Message = "Valid ID is required" });

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors.ToList()
                    });
                }

                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, dto);

                if (updatedEmployee == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update employee {Id}", id);
                return StatusCode(500, new { message = "Failed to update employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete employee (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse { Success = false, Message = "Valid ID is required" });

                var result = await _employeeService.DeleteEmployeeAsync(id);

                if (!result)
                    return NotFound(new { message = "Employee not found" });

                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete employee {Id}", id);
                return StatusCode(500, new { message = "Failed to delete employee", error = ex.Message });
            }
        }

        /// <summary>
        /// Get employee attendance report
        /// </summary>
        [HttpGet("{employeeId}/attendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttendanceReport(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (employeeId <= 0)
                    return BadRequest(new { success = false, message = "Valid Employee ID is required" });

                return Ok(await _reportService.GetEmployeeAttendanceAsync(employeeId, startDate, endDate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get attendance report for EmployeeId {EmployeeId}", employeeId);
                return StatusCode(500, new { message = "Failed to generate report", error = ex.Message });
            }
        }

        /// <summary>
        /// Download monthly report as CSV
        /// </summary>
        [HttpGet("{employeeId}/report/monthly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadMonthlyReport(int employeeId, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                if (employeeId <= 0)
                    return BadRequest(new { success = false, message = "Valid Employee ID is required" });

                var reportData = await _reportService.GenerateMonthlyReportAsync(employeeId, month, year);

                if (reportData.Length == 0)
                    return NotFound(new { success = false, message = "No data found for the specified period" });

                return File(reportData, "text/csv", $"punch_report_{month}_{year}.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download monthly report for EmployeeId {EmployeeId}, Month {Month}, Year {Year}", employeeId, month, year);
                return StatusCode(500, new { message = "Failed to download report", error = ex.Message });
            }
        }
    }
}