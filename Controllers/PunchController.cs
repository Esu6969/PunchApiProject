    using Microsoft.AspNetCore.Mvc;
using PunchApiProject.DTOs;
using PunchApiProject.Services.Interfaces;

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

        /// <summary>
        /// Punch in by Employee ID (string)
        /// </summary>
        [HttpPost("in/by-employeeid")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PunchInByEmployeeId([FromBody] PunchByEmployeeIdDto request)
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

            _logger.LogInformation("Punch in request for employee: {EmployeeId}", request.EmployeeId);
            return Ok(await _punchService.PunchInAsync(request.EmployeeId.GetHashCode()));
        }

        /// <summary>
        /// Punch out by Employee ID (string)
        /// </summary>
        [HttpPost("out/by-employeeid")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PunchOutByEmployeeId([FromBody] PunchByEmployeeIdDto request)
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

            _logger.LogInformation("Punch out request for employee: {EmployeeId}", request.EmployeeId);
            return Ok(await _punchService.PunchOutAsync(request.EmployeeId.GetHashCode()));
        }

        /// <summary>
        /// Get all punch records
        /// </summary>
        [HttpGet("records")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PunchRecordDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRecords()
        {
            return Ok(await _punchService.GetAllPunchRecordsAsync());
        }

        /// <summary>
        /// Get punch records for a specific employee
        /// </summary>
        [HttpGet("records/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PunchRecordDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecordsByEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Valid Employee ID is required"
                });
            }

            return Ok(await _punchService.GetPunchRecordsByEmployeeIdAsync(employeeId));
        }

        /// <summary>
        /// Get punch records by date range
        /// </summary>
        [HttpGet("records/{employeeId}/range")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PunchRecordDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecordsByDateRange(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (employeeId <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Valid Employee ID is required"
                });
            }

            return Ok(await _punchService.GetRecordsByDateRangeAsync(employeeId, startDate, endDate));
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        [HttpGet("stats/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeStatsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeStats(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Valid Employee ID is required"
                });
            }

            return Ok(await _punchService.GetEmployeeStatsAsync(employeeId));
        }
    }
}