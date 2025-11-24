using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using PunchApiProject.DTOs;
using System.Threading.Tasks;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        private readonly ILogger<PunchController> _logger;

        public PunchController(IPunchService punchService, ILogger<PunchController> logger)
        {
            _punchService = punchService;
            _logger = logger;
        }

        // POST: api/punch/in
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Punch in error for employee {EmployeeId}", request.EmployeeId);
                return StatusCode(500, new { message = "Punch in failed", error = ex.Message });
            }
        }

        // POST: api/punch/out
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Punch out error for employee {EmployeeId}", request.EmployeeId);
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get stats for employee {EmployeeId}", employeeId);
                return StatusCode(500, new { message = "Failed to retrieve stats", error = ex.Message });
            }
        }
    }

    // DTO for punch requests
    public class PunchRequestDto
    {
        public int EmployeeId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}