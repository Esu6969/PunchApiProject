using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;
using PunchApiProject.DTOs;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("all-activity")]
        public async Task<IActionResult> GetAllActivity()
        {
            var data = await _employeeService.GetAllEmployeeActivityAsync();
            return Ok(data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _employeeService.RegisterAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}