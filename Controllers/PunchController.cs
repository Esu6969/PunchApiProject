using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using PunchApiProject.Data;
using PunchApiProject.Models;
using PunchApiProject.DTOs;
using System.Threading.Tasks;
using System.Linq;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegisterDto dto)
        {
            var employee = await _punchService.RegisterEmployeeAsync(dto.Name, dto.Email);
            return Ok(employee);
        }

        [HttpPost("punch")]
        public async Task<IActionResult> Punch([FromBody] CreatePunchRecordDto dto)
        {
            var punch = await _punchService.AddPunchRecordAsync(dto.EmployeeId, dto.ActionType);
            return Ok(punch);
        }

        [HttpGet("records")]
        public async Task<IActionResult> GetAllPunchRecords()
        {
            var records = await _punchService.GetAllPunchRecordsAsync();
            return Ok(records);
        }
    }
}