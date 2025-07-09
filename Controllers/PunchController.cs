using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using PunchApiProject.DTOs;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;

        public PunchController(IPunchService punchService)
        {
            _punchService = punchService;
        }

        /// <summary>
        /// Punches in an employee with the current timestamp.
        /// </summary>
        /// <param name="request">DTO containing employee name</param>
        /// <returns>Punch record details</returns>
        [HttpPost("punchin")]
        public IActionResult PunchIn([FromBody] PunchRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeName))
                return BadRequest("Employee name is required.");

            var result = _punchService.PunchIn(request.EmployeeName);
            return Ok(result);
        }

        /// <summary>
        /// Punches out an employee based on PunchRecord ID.
        /// </summary>
        /// <param name="id">PunchRecord ID</param>
        /// <returns>Updated Punch record</returns>
        [HttpPost("punchout/{id}")]
        public IActionResult PunchOut(int id)
        {
            var result = _punchService.PunchOut(id);
            if (result == null)
                return NotFound("Punch record not found or already punched out.");

            return Ok(result);
        }

        /// <summary>
        /// Gets all punch records.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllPunches()
        {
            var punches = _punchService.GetAllPunchRecords();
            return Ok(punches);
        }

        /// <summary>
        /// Filters punch records by date (yyyy-MM-dd).
        /// </summary>
        /// <param name="date">Date to filter by</param>
        [HttpGet("filter")]
        public IActionResult FilterByDate([FromQuery] DateTime date)
        {
            var filtered = _punchService.GetPunchesByDate(date);
            return Ok(filtered);
        }

        /// <summary>
        /// Calculates total working hours for a punch record by ID.
        /// </summary>
        /// <param name="id">PunchRecord ID</param>
        [HttpGet("totalhours/{id}")]
        public IActionResult GetTotalHours(int id)
        {
            var hours = _punchService.CalculateTotalHours(id);
            if (hours == null)
                return NotFound("Punch record not found or PunchOut time is missing.");

            return Ok(new
            {
                Id = id,
                TotalHours = hours
            });
        }
    }
}
