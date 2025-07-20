using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using System;
using System.Threading.Tasks;

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

        [HttpGet("totalhours/{id}")]
        public async Task<IActionResult> GetTotalHours(int id)
        {
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
