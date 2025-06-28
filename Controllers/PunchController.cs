using Microsoft.AspNetCore.Mvc;
using PunchApiProject.Models;
using System.Text.Json;

namespace PunchApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PunchController : ControllerBase
    {
        private static List<PunchRecord> punchRecords = LoadPunchDataFromFile();
        private static int punchIdCounter = punchRecords.Count > 0 ? punchRecords.Max(p => p.PunchId) + 1 : 1;
        private static readonly string dataFilePath = "punch_data.json";

        // ✅ POST: Punch In / Out with Validation and File Save
        [HttpPost]
        public IActionResult Punch([FromBody] PunchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeName))
                return BadRequest("Employee name cannot be empty.");

            if (request.PunchType != "In" && request.PunchType != "Out")
                return BadRequest("PunchType must be 'In' or 'Out'.");

            var punchRecord = new PunchRecord
            {
                PunchId = punchIdCounter++,
                EmployeeName = request.EmployeeName,
                PunchTime = DateTime.Now,
                PunchType = request.PunchType
            };

            punchRecords.Add(punchRecord);
            SavePunchDataToFile();

            return Ok(punchRecord);
        }

        // ✅ GET: All Punches
        [HttpGet]
        public IActionResult GetAllPunches()
        {
            return Ok(punchRecords);
        }

        // ✅ GET: Filter by Employee Name
        [HttpGet("byName")]
        public IActionResult GetPunchesByName(string name)
        {
            var filtered = punchRecords
                .Where(p => p.EmployeeName.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filtered.Count == 0)
                return NotFound($"No punches found for employee: {name}");

            return Ok(filtered);
        }

        // ✅ GET: Filter by Date
        [HttpGet("byDate")]
        public IActionResult GetPunchesByDate(DateTime date)
        {
            var filtered = punchRecords
                .Where(p => p.PunchTime.Date == date.Date)
                .ToList();

            if (filtered.Count == 0)
                return NotFound($"No punches found for date: {date:yyyy-MM-dd}");

            return Ok(filtered);
        }

        // ✅ Helper: Save to JSON File
        private static void SavePunchDataToFile()
        {
            var json = JsonSerializer.Serialize(punchRecords, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(dataFilePath, json);
        }

        // ✅ Helper: Load from JSON File (if exists)
        private static List<PunchRecord> LoadPunchDataFromFile()
        {
            if (System.IO.File.Exists(dataFilePath))
            {
                var json = System.IO.File.ReadAllText(dataFilePath);
                var data = JsonSerializer.Deserialize<List<PunchRecord>>(json);
                return data ?? new List<PunchRecord>();
            }
            return new List<PunchRecord>();
        }
    }
}
