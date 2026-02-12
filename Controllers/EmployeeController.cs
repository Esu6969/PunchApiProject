// Controllers/EmployeeController.cs
// ✅ No manual session checks needed anywhere in this file
//    SessionMiddleware handles it automatically for all routes

using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using PunchApiProject.Models;
using PunchApiProject.DTOs;
using PunchApiProject.Middleware; // ✅ import for session extensions

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        // GET: api/employee
        // ✅ No session check needed — middleware handles it automatically
        [HttpGet]
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

        // GET: api/employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
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

        // POST: api/employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newEmployee = await _employeeService.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create employee");
                return StatusCode(500, new { message = "Failed to create employee", error = ex.Message });
            }
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.Id)
                    return BadRequest(new { message = "ID mismatch" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employee);

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

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
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

        // POST: api/employee/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegisterDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _employeeService.RegisterAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for {EmployeeId}", request.EmployeeId);
                return StatusCode(500, new { message = "Registration failed", error = ex.Message });
            }
        }

        // GET: api/employee/activities
        [HttpGet("activities")]
        public async Task<IActionResult> GetEmployeeActivities()
        {
            try
            {
                var activities = await _employeeService.GetAllEmployeeActivityAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get activities");
                return StatusCode(500, new { message = "Failed to retrieve activities", error = ex.Message });
            }
        }

        // GET: api/employee/me/info
        // ✅ Get current logged-in employee's own info using session
        [HttpGet("me/info")]
        public async Task<IActionResult> GetMyInfo()
        {
            try
            {
                // ✅ Read id from session — no need to pass id in URL
                var id = HttpContext.GetSessionId();

                if (id == null)
                    return Unauthorized(new { message = "Session invalid" });

                var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

                if (employee == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get own info");
                return StatusCode(500, new { message = "Failed to retrieve info", error = ex.Message });
            }
        }
    }
}