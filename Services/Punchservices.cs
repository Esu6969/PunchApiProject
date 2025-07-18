using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;

namespace PunchApiProject.Services
{
    public class PunchService : IPunchService
    {
        private readonly AppDbContext _context;

        public PunchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PunchRecord>> GetAllAsync()
        {
            return await _context.PunchRecords.ToListAsync();
        }

        public async Task<PunchRecord?> GetByIdAsync(int id)
        {
            return await _context.PunchRecords.FindAsync(id);
        }

        public async Task<PunchRecord> PunchInAsync(CreatePunchRecordDto dto)
        {
            var record = new PunchRecord
{
    EmployeeName = dto.EmployeeName,
    PunchIn = DateTime.UtcNow,          // Convert to UTC
    PunchOut = null                     // Or DateTime.UtcNow if punching out
};

            _context.PunchRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<PunchRecord?> PunchOutAsync(UpdatePunchOutDto dto)
        {
            var record = await _context.PunchRecords.FindAsync(dto.Id);
            if (record == null || record.PunchOut != null) return null;

            record.PunchOut = DateTime.Now;
            await _context.SaveChangesAsync();
            return record;
        }
    }
}
