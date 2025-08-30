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
        private readonly PunchDbContext _context;

        public PunchService(PunchDbContext context)
        {
            _context = context;
        }

        public async Task AddPunchRecordAsync(Punch punch)
        {
            try
            {
                _context.Punches.Add(punch);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log or rethrow with more details
                throw new Exception("Error saving Punch record: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Punch>> GetAllPunchRecordsAsync()
        {
            return await _context.Punches.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Punch>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Punches
                .Where(p => p.ActionDateTime.Date >= startDate.Date && p.ActionDateTime.Date <= endDate.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TimeSpan> CalculateTotalHoursAsync(int employeeId)
        {
            var records = await _context.Punches
                .Where(p => p.EmployeeId == employeeId && (p.ActionType == "PunchIn" || p.ActionType == "PunchOut"))
                .OrderBy(p => p.ActionDateTime)
                .AsNoTracking()
                .ToListAsync();

            TimeSpan total = TimeSpan.Zero;
            DateTime? lastIn = null;
            foreach (var r in records)
            {
                if (r.ActionType == "PunchIn")
                {
                    lastIn = r.ActionDateTime;
                }
                else if (r.ActionType == "PunchOut" && lastIn.HasValue)
                {
                    total += r.ActionDateTime - lastIn.Value;
                    lastIn = null;
                }
            }

            return total;
        }
    }
}