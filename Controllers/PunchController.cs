using Microsoft.AspNetCore.Mvc;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using PunchApiProject.Services;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _service;

        public PunchController(IPunchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PunchRecord>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PunchRecord>> Get(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        [HttpPost("punchIn")]
        public async Task<ActionResult<PunchRecord>> PunchIn([FromBody] CreatePunchRecordDto dto)
        {
            var result = await _service.PunchInAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPost("punchOut")]
        public async Task<ActionResult<PunchRecord>> PunchOut([FromBody] UpdatePunchOutDto dto)
        {
            var result = await _service.PunchOutAsync(dto);
            if (result == null) return NotFound("Record not found or already punched out.");
            return Ok(result);
        }
    }
}
