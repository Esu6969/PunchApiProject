using PunchApiProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public class PunchService : IPunchService
    {
        private static readonly string DataFilePath = "punch_data.json";
        private static List<PunchRecord> _punchRecords = LoadPunchDataFromFile();

        private static List<PunchRecord> LoadPunchDataFromFile()
        {
            if (!File.Exists(DataFilePath))
                return new List<PunchRecord>();

            var json = File.ReadAllText(DataFilePath);
            return JsonSerializer.Deserialize<List<PunchRecord>>(json) ?? new List<PunchRecord>();
        }

        private static void SavePunchDataToFile()
        {
            var json = JsonSerializer.Serialize(_punchRecords, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataFilePath, json);
        }

        public async Task PunchInAsync(int employeeId)
        {
            var punchInRecord = new PunchRecord
            {
                Id = _punchRecords.Count + 1,
                EmployeeId = employeeId,
                PunchInTime = DateTime.Now
            };

            _punchRecords.Add(punchInRecord);
            SavePunchDataToFile();

            await Task.CompletedTask;
        }

        public async Task PunchOutAsync(int employeeId)
        {
            var record = _punchRecords
                .Where(p => p.EmployeeId == employeeId && p.PunchOutTime == null)
                .OrderByDescending(p => p.PunchInTime)
                .FirstOrDefault();

            if (record != null)
            {
                record.PunchOutTime = DateTime.Now;
                SavePunchDataToFile();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync()
        {
            return await Task.FromResult(_punchRecords);
        }

        public async Task<IEnumerable<PunchRecord>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate)
        {
            var filtered = _punchRecords
                .Where(p => p.PunchInTime.Date >= startDate.Date && p.PunchInTime.Date <= endDate.Date)
                .ToList();

            return await Task.FromResult(filtered);
        }

        public async Task<TimeSpan> CalculateTotalHoursAsync(int employeeId)
        {
            var employeeRecords = _punchRecords
                .Where(p => p.EmployeeId == employeeId && p.PunchOutTime.HasValue)
                .ToList();

            TimeSpan totalDuration = TimeSpan.Zero;

            foreach (var record in employeeRecords)
            {
                totalDuration += record.PunchOutTime.Value - record.PunchInTime;
            }

            return await Task.FromResult(totalDuration);
        }
    }
}
