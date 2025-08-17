using PunchApiProject.Models;
using PunchApiProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public class PunchService : IPunchService
    {
        private readonly PunchDbContext _context;

        public PunchService(PunchDbContext context)
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
            _context.Punches.Add(punch);
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
            return await _context.Punches.ToListAsync();
        }

        public async Task<IEnumerable<Punch>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Punches
                .Where(p => p.PunchIn.Date >= startDate.Date && p.PunchIn.Date <= endDate.Date)
                .ToListAsync();
        }

        public async Task<TimeSpan> CalculateTotalHoursAsync(int employeeId)
        {
            var records = await _context.Punches
                .Where(p => p.EmployeeId == employeeId && p.PunchOut.HasValue)
                .ToListAsync();

            TimeSpan total = TimeSpan.Zero;
            foreach (var record in records)
            {
                total += record.PunchOut.Value - record.PunchIn;
            }
            return total;
        }
    }
}
            
