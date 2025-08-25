using PunchApiProject.Models;
using PunchApiProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    /// <summary>Handles punch-in/out logic and queries.</summary>
    public class PunchService : IPunchService
    {
        private readonly AppDbContext _context;

        public PunchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task PunchInAsync(int employeeId)
        {
            var punch = new Punch
            {
                EmployeeId = employeeId,
                PunchIn = DateTime.Now
            };

            await _context.Punches.AddAsync(punch);
            await _context.SaveChangesAsync();
        }

        public async Task PunchOutAsync(int employeeId)
        {
            var record = await _context.Punches
                .Where(p => p.EmployeeId == employeeId && p.PunchOut == null)
                .OrderByDescending(p => p.PunchIn)
                .FirstOrDefaultAsync();

            if (record != null)
            {
                record.PunchOut = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Punch>> GetAllPunchRecordsAsync()
        {
            return await _context.Punches.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Punch>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Punches
                .Where(p => p.PunchIn.Date >= startDate.Date && p.PunchIn.Date <= endDate.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TimeSpan> CalculateTotalHoursAsync(int employeeId)
        {
            var records = await _context.Punches
                .Where(p => p.EmployeeId == employeeId && p.PunchOut.HasValue)
                .AsNoTracking()
                .ToListAsync();

            TimeSpan total = TimeSpan.Zero;
            foreach (var r in records)
                total += r.PunchOut!.Value - r.PunchIn;

            return total;
        }
    }
}