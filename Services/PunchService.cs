    using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.DTOs;
using PunchApiProject.Models;
using PunchApiProject.Services.Interfaces;

namespace PunchApiProject.Services
{
    public class PunchService : IPunchService
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<PunchService> _logger;
        private readonly IAuditService _auditService;

        public PunchService(PunchDbContext context, ILogger<PunchService> logger, IAuditService auditService)
        {
            _context = context;
            _logger = logger;
            _auditService = auditService;
        }

        /// <summary>
        /// Get all punch records for all employees
        /// </summary>
        public async Task<ApiResponse<IEnumerable<PunchRecordDto>>> GetAllPunchRecordsAsync()
        {
            var response = new ApiResponse<IEnumerable<PunchRecordDto>>();

            try
            {
                var records = await _context.PunchRecords
                    .Include(p => p.Employee)
                    .OrderByDescending(p => p.ActionDateTime)
                    .Select(p => new PunchRecordDto
                    {
                        Id = p.Id,
                        EmployeeId = p.EmployeeId,
                        EmployeeName = $"{p.Employee!.FirstName} {p.Employee.LastName}",
                        ActionDateTime = p.ActionDateTime,
                        ActionType = p.ActionType
                    })
                    .ToListAsync();

                response.Success = true;
                response.Message = $"Retrieved {records.Count} punch records";
                response.Data = records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all punch records");
                response.Success = false;
                response.Message = "Failed to retrieve punch records";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get punch records for a specific employee
        /// </summary>
        public async Task<ApiResponse<IEnumerable<PunchRecordDto>>> GetPunchRecordsByEmployeeIdAsync(int employeeId)
        {
            var response = new ApiResponse<IEnumerable<PunchRecordDto>>();

            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                var records = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .Include(p => p.Employee)
                    .OrderByDescending(p => p.ActionDateTime)
                    .Select(p => new PunchRecordDto
                    {
                        Id = p.Id,
                        EmployeeId = p.EmployeeId,
                        EmployeeName = $"{p.Employee!.FirstName} {p.Employee.LastName}",
                        ActionDateTime = p.ActionDateTime,
                        ActionType = p.ActionType
                    })
                    .ToListAsync();

                response.Success = true;
                response.Message = $"Retrieved {records.Count} records for {employee.FullName}";
                response.Data = records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving punch records for employee {EmployeeId}", employeeId);
                response.Success = false;
                response.Message = "Failed to retrieve punch records";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get punch record by ID
        /// </summary>
        public async Task<ApiResponse<PunchRecordDto>> GetPunchRecordByIdAsync(int id)
        {
            var response = new ApiResponse<PunchRecordDto>();

            try
            {
                var record = await _context.PunchRecords
                    .Include(p => p.Employee)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (record == null)
                {
                    response.Success = false;
                    response.Message = "Punch record not found";
                    return response;
                }

                response.Success = true;
                response.Data = new PunchRecordDto
                {
                    Id = record.Id,
                    EmployeeId = record.EmployeeId,
                    EmployeeName = $"{record.Employee!.FirstName} {record.Employee.LastName}",
                    ActionDateTime = record.ActionDateTime,
                    ActionType = record.ActionType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving punch record {RecordId}", id);
                response.Success = false;
                response.Message = "Failed to retrieve punch record";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Punch in for an employee
        /// </summary>
        public async Task<ApiResponse<object>> PunchInAsync(int employeeId)
        {
            var response = new ApiResponse<object>();

            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsActive);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found or inactive";
                    return response;
                }

                // Check if already punched in
                var lastAction = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .OrderByDescending(p => p.ActionDateTime)
                    .FirstOrDefaultAsync();

                if (lastAction != null && lastAction.ActionType.Equals("PunchIn", StringComparison.OrdinalIgnoreCase))
                {
                    response.Success = false;
                    response.Message = $"{employee.FullName} is already punched in. Please punch out first.";
                    return response;
                }

                var punchRecord = new PunchRecord
                {
                    EmployeeId = employeeId,
                    ActionDateTime = DateTime.UtcNow,
                    ActionType = "PunchIn"
                };

                _context.PunchRecords.Add(punchRecord);
                await _context.SaveChangesAsync();

                // Update last login
                employee.LastLoginAt = DateTime.UtcNow;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                // Log audit
                await _auditService.LogActionAsync(employeeId, "PunchIn", "Employee punched in");

                response.Success = true;
                response.Message = $"{employee.FullName} punched in successfully at {punchRecord.ActionDateTime:yyyy-MM-dd HH:mm:ss}";
                response.Data = new
                {
                    punchId = punchRecord.Id,
                    employeeId = employee.Id,
                    employeeName = employee.FullName,
                    actionDateTime = punchRecord.ActionDateTime,
                    actionType = punchRecord.ActionType
                };

                _logger.LogInformation("Employee {EmployeeId} ({Name}) punched in", employeeId, employee.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Punch in failed for employee {EmployeeId}", employeeId);
                response.Success = false;
                response.Message = "Punch in operation failed";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Punch out for an employee
        /// </summary>
        public async Task<ApiResponse<object>> PunchOutAsync(int employeeId)
        {
            var response = new ApiResponse<object>();

            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsActive);

                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found or inactive";
                    return response;
                }

                var lastAction = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .OrderByDescending(p => p.ActionDateTime)
                    .FirstOrDefaultAsync();

                if (lastAction == null || lastAction.ActionType.Equals("PunchOut", StringComparison.OrdinalIgnoreCase))
                {
                    response.Success = false;
                    response.Message = $"{employee.FullName} is not punched in. Please punch in first.";
                    return response;
                }

                var punchOutRecord = new PunchRecord
                {
                    EmployeeId = employeeId,
                    ActionDateTime = DateTime.UtcNow,
                    ActionType = "PunchOut"
                };

                _context.PunchRecords.Add(punchOutRecord);
                await _context.SaveChangesAsync();

                // Log audit
                await _auditService.LogActionAsync(employeeId, "PunchOut", "Employee punched out");

                var timeSpan = punchOutRecord.ActionDateTime - lastAction.ActionDateTime;
                var hoursWorked = timeSpan.TotalHours;

                response.Success = true;
                response.Message = $"{employee.FullName} punched out successfully. Hours worked: {hoursWorked:F2}";
                response.Data = new
                {
                    punchId = punchOutRecord.Id,
                    employeeId = employee.Id,
                    employeeName = employee.FullName,
                    punchInTime = lastAction.ActionDateTime,
                    punchOutTime = punchOutRecord.ActionDateTime,
                    hoursWorked = Math.Round(hoursWorked, 2),
                    actionType = punchOutRecord.ActionType
                };

                _logger.LogInformation("Employee {EmployeeId} ({Name}) punched out. Hours: {Hours}", employeeId, employee.FullName, hoursWorked);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Punch out failed for employee {EmployeeId}", employeeId);
                response.Success = false;
                response.Message = "Punch out operation failed";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        public async Task<ApiResponse<EmployeeStatsDto>> GetEmployeeStatsAsync(int employeeId)
        {
            var response = new ApiResponse<EmployeeStatsDto>();

            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                var today = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                var monthStart = new DateTime(today.Year, today.Month, 1);

                var todayRecords = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId && p.ActionDateTime.Date == today)
                    .OrderBy(p => p.ActionDateTime)
                    .ToListAsync();

                var weekRecords = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId && p.ActionDateTime >= weekStart && p.ActionDateTime <= today.AddDays(1))
                    .OrderBy(p => p.ActionDateTime)
                    .ToListAsync();

                var monthRecords = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId && p.ActionDateTime.Year == today.Year && p.ActionDateTime.Month == today.Month)
                    .OrderBy(p => p.ActionDateTime)
                    .ToListAsync();

                var lastRecord = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId)
                    .OrderByDescending(p => p.ActionDateTime)
                    .FirstOrDefaultAsync();

                var stats = new EmployeeStatsDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    TodayHours = CalculateHours(todayRecords),
                    WeekHours = CalculateHours(weekRecords),
                    MonthHours = CalculateHours(monthRecords),
                    TodayPunches = todayRecords.Count,
                    LastPunchTime = lastRecord?.ActionDateTime,
                    LastPunchType = lastRecord?.ActionType ?? "None"
                };

                response.Success = true;
                response.Message = "Statistics retrieved successfully";
                response.Data = stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating stats for employee {EmployeeId}", employeeId);
                response.Success = false;
                response.Message = "Failed to retrieve statistics";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get records by date range
        /// </summary>
        public async Task<ApiResponse<IEnumerable<PunchRecordDto>>> GetRecordsByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var response = new ApiResponse<IEnumerable<PunchRecordDto>>();

            try
            {
                if (endDate < startDate)
                {
                    response.Success = false;
                    response.Message = "End date cannot be before start date";
                    return response;
                }

                var records = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId && 
                                p.ActionDateTime >= startDate && 
                                p.ActionDateTime <= endDate.AddDays(1))
                    .Include(p => p.Employee)
                    .OrderByDescending(p => p.ActionDateTime)
                    .Select(p => new PunchRecordDto
                    {
                        Id = p.Id,
                        EmployeeId = p.EmployeeId,
                        EmployeeName = $"{p.Employee!.FirstName} {p.Employee.LastName}",
                        ActionDateTime = p.ActionDateTime,
                        ActionType = p.ActionType
                    })
                    .ToListAsync();

                response.Success = true;
                response.Message = $"Retrieved {records.Count} records between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}";
                response.Data = records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving records for date range");
                response.Success = false;
                response.Message = "Failed to retrieve records";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        private decimal CalculateHours(List<PunchRecord> records)
        {
            decimal totalHours = 0;

            for (int i = 0; i < records.Count - 1; i += 2)
            {
                if (records[i].ActionType.Equals("PunchIn", StringComparison.OrdinalIgnoreCase) &&
                    i + 1 < records.Count &&
                    records[i + 1].ActionType.Equals("PunchOut", StringComparison.OrdinalIgnoreCase))
                {
                    var duration = records[i + 1].ActionDateTime - records[i].ActionDateTime;
                    totalHours += (decimal)duration.TotalHours;
                }
            }

            return Math.Round(totalHours, 2);
        }
    }
}