using PunchApiProject.Data;
using PunchApiProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<Employee> RegisterEmployeeAsync(string name, string email)
        {
            var employee = new Employee { Name = name, Email = email };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<PunchRecord> AddPunchRecordAsync(int employeeId, string actionType)
        {
            var punch = new PunchRecord
            {
                EmployeeId = employeeId,
                ActionDateTime = DateTime.UtcNow,
                ActionType = actionType
            };
            _context.PunchRecords.Add(punch);
            await _context.SaveChangesAsync();
            return punch;
        }

        public async Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync()
        {
            return await _context.PunchRecords.Include(p => p.Employee).ToListAsync();
        }
    }
}