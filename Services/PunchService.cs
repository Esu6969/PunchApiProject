using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Models;
using PunchApiProject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public class PunchService : IPunchService
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<PunchService> _logger;

        public PunchService(PunchDbContext context, ILogger<PunchService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all punch records
        public async Task<IEnumerable<PunchRecord>> GetAllPunchRecordsAsync()
        {
            return await _context.PunchRecords
                .Include(p => p.Employee)
                .OrderByDescending(p => p.ActionDateTime)
                .ToListAsync();
        }

        // Get punch records by employee ID
        public async Task<IEnumerable<PunchRecord>> GetPunchRecordsByEmployeeIdAsync(int employeeId)
        {
            return await _context.PunchRecords
                .Where(p => p.EmployeeId == employeeId)
                .Include(p => p.Employee)
                .OrderByDescending(p => p.ActionDateTime)
                .ToListAsync();
        }

        // Get punch record by ID
        public async Task<PunchRecord?> GetPunchRecordByIdAsync(int id)
        {
            return await _context.PunchRecords
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Punch In
        public async Task<ApiResponse> PunchInAsync(int employeeId)
        {
            var response = new ApiResponse();

            try
            {
                // Check if employee exists
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsActive);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found or inactive";
                    return response;
                }

                // Check if already punched in (last action was PunchIn)
                var lastAction = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .OrderByDescending(p => p.ActionDateTime)
                    .FirstOrDefaultAsync();

                if (lastAction != null && lastAction.ActionType == "PunchIn")
                {
                    response.Success = false;
                    response.Message = "Already punched in. Please punch out first.";
                    return response;
                }

                // Create new punch in record
                var punchRecord = new PunchRecord
                {
                    EmployeeId = employeeId,
                    ActionDateTime = DateTime.Now,
                    ActionType = "PunchIn"
                };

                _context.PunchRecords.Add(punchRecord);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = $"{employee.FirstName} {employee.LastName} punched in successfully at {punchRecord.ActionDateTime:hh:mm tt}";
                response.Data = new
                {
                    punchId = punchRecord.Id,
                    employeeId = employee.Id,
                    employeeName = $"{employee.FirstName} {employee.LastName}",
                    actionDateTime = punchRecord.ActionDateTime,
                    actionType = punchRecord.ActionType
                };

                _logger.LogInformation("Employee {EmployeeId} punched in at {Time}", employeeId, punchRecord.ActionDateTime);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Punch in failed: {ex.Message}";
                _logger.LogError(ex, "Punch in failed for employee {EmployeeId}", employeeId);
            }

            return response;
        }

        // Punch Out
        public async Task<ApiResponse> PunchOutAsync(int employeeId)
        {
            var response = new ApiResponse();

            try
            {
                // Check if employee exists
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsActive);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found or inactive";
                    return response;
                }

                // Check last action
                var lastAction = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .OrderByDescending(p => p.ActionDateTime)
                    .FirstOrDefaultAsync();

                if (lastAction == null || lastAction.ActionType == "PunchOut")
                {
                    response.Success = false;
                    response.Message = "No active punch in found. Please punch in first.";
                    return response;
                }

                // Create punch out record
                var punchOutRecord = new PunchRecord
                {
                    EmployeeId = employeeId,
                    ActionDateTime = DateTime.Now,
                    ActionType = "PunchOut"
                };

                _context.PunchRecords.Add(punchOutRecord);
                await _context.SaveChangesAsync();

                // Calculate hours worked
                var timeSpan = punchOutRecord.ActionDateTime - lastAction.ActionDateTime;
                var hoursWorked = timeSpan.TotalHours;

                response.Success = true;
                response.Message = $"{employee.FirstName} {employee.LastName} punched out successfully. Hours worked: {hoursWorked:F2}";
                response.Data = new
                {
                    punchId = punchOutRecord.Id,
                    employeeId = employee.Id,
                    employeeName = $"{employee.FirstName} {employee.LastName}",
                    punchInTime = lastAction.ActionDateTime,
                    punchOutTime = punchOutRecord.ActionDateTime,
                    hoursWorked = Math.Round(hoursWorked, 2),
                    actionType = punchOutRecord.ActionType
                };

                _logger.LogInformation("Employee {EmployeeId} punched out at {Time}. Hours: {Hours}", 
                    employeeId, punchOutRecord.ActionDateTime, hoursWorked);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Punch out failed: {ex.Message}";
                _logger.LogError(ex, "Punch out failed for employee {EmployeeId}", employeeId);
            }

            return response;
        }

        // Get employee statistics
        public async Task<object> GetEmployeeStatsAsync(int employeeId)
        {
            try
            {
                var today = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                var monthStart = new DateTime(today.Year, today.Month, 1);

                // Get all punch records for calculations
                var allRecords = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .OrderBy(p => p.ActionDateTime)
                    .ToListAsync();

                // Calculate today's hours
                var todayRecords = allRecords.Where(p => p.ActionDateTime.Date == today).ToList();
                var todayHours = CalculateHours(todayRecords);

                // Calculate week hours
                var weekRecords = allRecords.Where(p => p.ActionDateTime.Date >= weekStart && p.ActionDateTime.Date <= today).ToList();
                var weekHours = CalculateHours(weekRecords);

                // Calculate month hours
                var monthRecords = allRecords.Where(p => p.ActionDateTime.Date >= monthStart && p.ActionDateTime.Date <= today).ToList();
                var monthHours = CalculateHours(monthRecords);

                // Calculate days worked
                var daysWorked = allRecords
                    .Select(p => p.ActionDateTime.Date)
                    .Distinct()
                    .Count();

                return new
                {
                    hoursToday = Math.Round(todayHours, 2),
                    hoursWeek = Math.Round(weekHours, 2),
                    hoursMonth = Math.Round(monthHours, 2),
                    totalDaysWorked = daysWorked,
                    attendanceRate = "98%"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get stats for employee {EmployeeId}", employeeId);
                return new
                {
                    hoursToday = 0.0,
                    hoursWeek = 0.0,
                    hoursMonth = 0.0,
                    totalDaysWorked = 0,
                    attendanceRate = "0%"
                };
            }
        }

        // Helper method to calculate total hours from punch records
        private double CalculateHours(List<PunchRecord> records)
        {
            double totalHours = 0;
            PunchRecord? lastPunchIn = null;

            foreach (var record in records.OrderBy(r => r.ActionDateTime))
            {
                if (record.ActionType == "PunchIn")
                {
                    lastPunchIn = record;
                }
                else if (record.ActionType == "PunchOut" && lastPunchIn != null)
                {
                    var timeSpan = record.ActionDateTime - lastPunchIn.ActionDateTime;
                    totalHours += timeSpan.TotalHours;
                    lastPunchIn = null;
                }
            }

            return totalHours;
        }
    }
}