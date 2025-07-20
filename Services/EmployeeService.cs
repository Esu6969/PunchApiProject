using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmployeeFullRecordDto>> GetAllEmployeeActivityAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.Logins)
            .Include(e => e.PunchRecords)
            .ToListAsync();

        var result = employees.Select(e => new EmployeeFullRecordDto
        {
            EmployeeId = e.Id,
            Username = e.Username,
            RegistrationDate = e.RegistrationDate,
            LoginTimes = e.Logins?.Select(l => l.LoginTime).ToList() ?? new List<DateTime>(),
            PunchRecords = e.PunchRecords?.Select(p => new PunchDto
            {
                PunchInTime = p.PunchInTime,
                PunchOutTime = p.PunchOutTime
            }).ToList() ?? new List<PunchDto>()
        }).ToList();

        return result;
    }
}