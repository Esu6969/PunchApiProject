using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Data;
using PunchApiProject.Models;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PunchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPunchRecords()
        {
            var records = _context.PunchRecords.ToList();
            return Ok(records);
        }

        [HttpPost("punchin")]
        public IActionResult PunchIn([FromBody] string employeeName)
        {
            var punchIn = new PunchRecord
            {
                EmployeeName = employeeName,
                PunchInTime = DateTime.Now
            };

            _context.PunchRecords.Add(punchIn);
            _context.SaveChanges();

            return Ok(punchIn);
        }

        [HttpPost("punchout/{id}")]
        public IActionResult PunchOut(int id)
        {
            var record = _context.PunchRecords.Find(id);
            if (record == null) return NotFound();

            record.PunchOutTime = DateTime.Now;
            _context.SaveChanges();

            return Ok(record);
        }
    }
}
