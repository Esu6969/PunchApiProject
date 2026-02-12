using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchApiProject.Services;
using PunchApiProject.DTOs;
using PunchApiProject.Data;
using System.Threading.Tasks;

namespace PunchApiProject.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        private readonly ILogger<PunchController> _logger;
        private readonly PunchDbContext _dbContext;

        public PunchController(IPunchService punchService, ILogger<PunchController> logger, PunchDbContext dbContext)
        {
            _punchService = punchService;
            _logger = logger;
            _dbContext = dbContext;
        }

        // POST: api/punch/in (existing - expects integer EmployeeId)
        [HttpPost("in")]
        public async Task<IActionResult> PunchIn([FromBody] PunchRequestDto request)
        {
            try
            {
                if (request.EmployeeId <= 0)
                {
                    return BadRequest(new { message = "Valid Employee ID is required" });
                }

                var result = await _punchService.PunchInAsync(request.EmployeeId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Punch in error for employee {EmployeeId}", request.EmployeeId);
                return StatusCode(500, new { message = "Punch in failed", error = ex.Message });
            }
        }

        // POST: api/punch/in/by-employeeid (new) - accepts string EmployeeId from frontend
        [HttpPost("in/by-employeeid")]
        public async Task<IActionResult> PunchInByEmployeeId([FromBody] PunchByEmployeeIdDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.EmployeeId))
                {
                    return BadRequest(new { message = "EmployeeId is required" });
                }

                var employee = await _dbContext.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId && e.IsActive);

                if (employee == null)
                {
                    return Unauthorized(new { success = false, message = "Employee not found or inactive" });
                }

                var result = await _punchService.PunchInAsync(employee.Id);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Punch in by EmployeeId failed for {EmployeeId}", request?.EmployeeId);
                return StatusCode(500, new { message = "Punch in failed", error = ex.Message });
            }
        }

        // POST: api/punch/out (existing - expects integer EmployeeId)
        [HttpPost("out")]
        public async Task<IActionResult> PunchOut([FromBody] PunchRequestDto request)
        {
            try
            {
                if (request.EmployeeId <= 0)
                {
                    return BadRequest(new { message = "Valid Employee ID is required" });
                }

                var result = await _punchService.PunchOutAsync(request.EmployeeId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Punch out error for employee {EmployeeId}", request.EmployeeId);
                return StatusCode(500, new { message = "Punch out failed", error = ex.Message });
            }
        }

        // POST: api/punch/out/by-employeeid (new) - accepts string EmployeeId from frontend
        [HttpPost("out/by-employeeid")]
        public async Task<IActionResult> PunchOutByEmployeeId([FromBody] PunchByEmployeeIdDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.EmployeeId))
                {
                    return BadRequest(new { message = "EmployeeId is required" });
                }

                var employee = await _dbContext.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId && e.IsActive);

                if (employee == null)
                {
                    return Unauthorized(new { success = false, message = "Employee not found or inactive" });
                }

                var result = await _punchService.PunchOutAsync(employee.Id);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Punch out by EmployeeId failed for {EmployeeId}", request?.EmployeeId);
                return StatusCode(500, new { message = "Punch out failed", error = ex.Message });
            }
        }

        // GET: api/punch/records
        [HttpGet("records")]
        public async Task<IActionResult> GetAllRecords()
        {
            try
            {
                var records = await _punchService.GetAllPunchRecordsAsync();
                return Ok(records);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get punch records");
                return StatusCode(500, new { message = "Failed to retrieve records", error = ex.Message });
            }
        }

        // GET: api/punch/records/{employeeId}
        [HttpGet("records/{employeeId}")]
        public async Task<IActionResult> GetRecordsByEmployee(int employeeId)
        {
            try
            {
                var records = await _punchService.GetPunchRecordsByEmployeeIdAsync(employeeId);
                return Ok(records);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get records for employee {EmployeeId}", employeeId);
                return StatusCode(500, new { message = "Failed to retrieve records", error = ex.Message });
            }
        }

        // GET: api/punch/stats/{employeeId}
        [HttpGet("stats/{employeeId}")]
        public async Task<IActionResult> GetEmployeeStats(int employeeId)
        {
            try
            {
                var stats = await _punchService.GetEmployeeStatsAsync(employeeId);
                return Ok(stats);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get stats for employee {EmployeeId}", employeeId);
                return StatusCode(500, new { message = "Failed to retrieve stats", error = ex.Message });
            }
        }
    }

    // DTO for punch requests (existing)
    public class PunchRequestDto
    {
        public int EmployeeId { get; set; }
        public string Employee_Id { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    // DTO used by new endpoints that accept string EmployeeId from frontend
    public class PunchByEmployeeIdDto
    {
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime? Timestamp { get; set; }
    }
}