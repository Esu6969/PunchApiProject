using PunchApiProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PunchApiProject.Data; // <-- For AppDbContext


namespace PunchApiProject.Services
{
    public class PunchService : IPunchService
    {
        private readonly AppDbContext _context;

        public PunchService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Punches in an employee with the current timestamp.
        /// </summary>
        /// <param name="employeeName">The name of the employee.</param>
        /// <returns>The created PunchRecord.</returns>
        public PunchRecord PunchIn(string employeeName)
        {
            var record = new PunchRecord
            {
                EmployeeName = employeeName,
                PunchInTime = DateTime.Now
            };

            _context.PunchRecords.Add(record);
            _context.SaveChanges();

            return record;
        }

        /// <summary>
        /// Punches out an employee based on PunchRecord ID.
        /// </summary>
        /// <param name="id">PunchRecord ID.</param>
        /// <returns>Updated PunchRecord or null if not found/invalid.</returns>
        public PunchRecord PunchOut(int id)
        {
            var record = _context.PunchRecords.FirstOrDefault(r => r.Id == id);
            if (record == null || record.PunchOutTime.HasValue)
                return null;

            record.PunchOutTime = DateTime.Now;
            _context.SaveChanges();

            return record;
        }

        /// <summary>
        /// Gets all punch records.
        /// </summary>
        /// <returns>List of PunchRecord.</returns>
        public List<PunchRecord> GetAllPunchRecords()
        {
            return _context.PunchRecords.ToList();
        }

        /// <summary>
        /// Gets punch records for a specific date.
        /// </summary>
        /// <param name="date">Date to filter records.</param>
        /// <returns>List of PunchRecord filtered by date.</returns>
        public List<PunchRecord> GetPunchesByDate(DateTime date)
        {
            return _context.PunchRecords
                .Where(r => r.PunchInTime.Date == date.Date)
                .ToList();
        }

        /// <summary>
        /// Calculates total working hours for a specific punch record.
        /// </summary>
        /// <param name="id">PunchRecord ID.</param>
        /// <returns>Total hours worked (TimeSpan) or null if invalid.</returns>
        public TimeSpan? CalculateTotalHours(int id)
        {
            var record = _context.PunchRecords.FirstOrDefault(r => r.Id == id);
            if (record == null || record.PunchOutTime == null)
                return null;

            return record.PunchOutTime.Value - record.PunchInTime;
        }
    }
}
