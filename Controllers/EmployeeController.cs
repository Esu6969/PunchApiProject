using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Services;

namespace PunchApiProject.Controllers
{
    // [Authorize] //
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
    }
}