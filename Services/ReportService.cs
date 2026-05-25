using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Services.Interfaces;
using System.Text;

namespace PunchApiProject.Services
{
    public class ReportService : IReportService
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<ReportService> _logger;

        public ReportService(PunchDbContext context, ILogger<ReportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Generate monthly report for an employee (CSV format)
        /// </summary>
        public async Task<byte[]> GenerateMonthlyReportAsync(int employeeId, int month, int year)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return Array.Empty<byte>();

                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var records = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId && 
                                p.ActionDateTime >= startDate && 
                                p.ActionDateTime <= endDate)
                    .OrderBy(p => p.ActionDateTime)
                    .ToListAsync();

                return GenerateCsvReport(employee, records, startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating monthly report for employee {EmployeeId}", employeeId);
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Generate department report
        /// </summary>
        public async Task<byte[]> GenerateDepartmentReportAsync(string department, DateTime startDate, DateTime endDate)
        {
            try
            {
                var employees = await _context.Employees
                    .Where(e => e.Department == department && e.IsActive)
                    .ToListAsync();

                if (employees.Count == 0)
                    return Array.Empty<byte>();

                var sb = new StringBuilder();
                sb.AppendLine($"Department Report - {department}");
                sb.AppendLine($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                sb.AppendLine();
                sb.AppendLine("Employee ID,Full Name,Total Hours,Days Worked");

                foreach (var employee in employees)
                {
                    var records = await _context.PunchRecords
                        .Where(p => p.EmployeeId == employee.Id && 
                                    p.ActionDateTime >= startDate && 
                                    p.ActionDateTime <= endDate)
                        .OrderBy(p => p.ActionDateTime)
                        .ToListAsync();

                    var (totalHours, daysWorked) = CalculateHoursAndDays(records);
                    sb.AppendLine($"{employee.EmployeeId},{employee.FullName},{totalHours:F2},{daysWorked}");
                }

                return Encoding.UTF8.GetBytes(sb.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating department report for {Department}", department);
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Get employee attendance data
        /// </summary>
        public async Task<object> GetEmployeeAttendanceAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return new { success = false, message = "Employee not found" };

                var records = await _context.PunchRecords
                    .Where(p => p.EmployeeId == employeeId && 
                                p.ActionDateTime >= startDate && 
                                p.ActionDateTime <= endDate)
                    .OrderBy(p => p.ActionDateTime)
                    .ToListAsync();

                var (totalHours, daysWorked) = CalculateHoursAndDays(records);

                return new
                {
                    success = true,
                    employeeId = employee.EmployeeId,
                    employeeName = employee.FullName,
                    period = new
                    {
                        startDate = startDate.ToString("yyyy-MM-dd"),
                        endDate = endDate.ToString("yyyy-MM-dd")
                    },
                    statistics = new
                    {
                        totalHours = Math.Round(totalHours, 2),
                        daysWorked,
                        recordCount = records.Count
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating attendance for employee {EmployeeId}", employeeId);
                return new { success = false, message = "Failed to retrieve attendance data", error = ex.Message };
            }
        }

        private byte[] GenerateCsvReport(Models.Employee employee, List<Models.PunchRecord> records, DateTime startDate, DateTime endDate)
        {
            var sb = new StringBuilder();

            // Header
            sb.AppendLine("Employee Punch Report");
            sb.AppendLine($"Employee: {employee.FullName}");
            sb.AppendLine($"Employee ID: {employee.EmployeeId}");
            sb.AppendLine($"Department: {employee.Department}");
            sb.AppendLine($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
            sb.AppendLine();

            // Column headers
            sb.AppendLine("Date,Time,Action");

            // Data
            foreach (var record in records)
            {
                sb.AppendLine($"{record.ActionDateTime:yyyy-MM-dd},{record.ActionDateTime:HH:mm:ss},{record.ActionType}");
            }

            // Summary
            var (totalHours, daysWorked) = CalculateHoursAndDays(records);
            sb.AppendLine();
            sb.AppendLine($"Total Hours,{totalHours:F2}");
            sb.AppendLine($"Days Worked,{daysWorked}");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private (decimal hours, int daysWorked) CalculateHoursAndDays(List<Models.PunchRecord> records)
        {
            decimal totalHours = 0;
            var uniqueDays = new HashSet<DateTime>();

            for (int i = 0; i < records.Count - 1; i += 2)
            {
                if (records[i].ActionType.Equals("PunchIn", StringComparison.OrdinalIgnoreCase) &&
                    i + 1 < records.Count &&
                    records[i + 1].ActionType.Equals("PunchOut", StringComparison.OrdinalIgnoreCase))
                {
                    var duration = records[i + 1].ActionDateTime - records[i].ActionDateTime;
                    totalHours += (decimal)duration.TotalHours;
                    uniqueDays.Add(records[i].ActionDateTime.Date);
                }
            }

            return (totalHours, uniqueDays.Count);
        }
    }
}