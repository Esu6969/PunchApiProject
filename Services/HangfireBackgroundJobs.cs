using Hangfire;
using PunchApiProject.Data;
using Microsoft.EntityFrameworkCore;

namespace PunchApiProject.Services
{
    public class HangfireBackgroundJobs
    {
        /// <summary>
        /// Schedule daily punch reminders
        /// </summary>
        [AutomaticRetry(Attempts = 3)]
        public static async Task SendDailyPunchRemindersAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<PunchDbContext>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundJobs>>();

                    // Get employees who haven't punched in today
                    var today = DateTime.Today;
                    var activeEmployees = await context.Employees
                        .Where(e => e.IsActive)
                        .ToListAsync();

                    var employeesNotPunchedIn = new List<int>();

                    foreach (var employee in activeEmployees)
                    {
                        var hasPunchedInToday = await context.PunchRecords
                            .AnyAsync(p => p.EmployeeId == employee.Id && 
                                          p.ActionDateTime.Date == today && 
                                          p.ActionType == "PunchIn");

                        if (!hasPunchedInToday)
                            employeesNotPunchedIn.Add(employee.Id);
                    }

                    logger.LogInformation("Daily punch reminder: {Count} employees haven't punched in yet", employeesNotPunchedIn.Count);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundJobs>>();
                    logger.LogError(ex, "Error sending daily punch reminders");
                    throw;
                }
            }
        }

        /// <summary>
        /// Clean up old audit logs (older than 90 days)
        /// </summary>
        [AutomaticRetry(Attempts = 3)]
        public static async Task CleanupOldAuditLogsAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<PunchDbContext>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundJobs>>();

                    var cutoffDate = DateTime.UtcNow.AddDays(-90);

                    var oldLogs = await context.AuditLogs
                        .Where(a => a.CreatedAt < cutoffDate)
                        .ToListAsync();

                    if (oldLogs.Count > 0)
                    {
                        context.AuditLogs.RemoveRange(oldLogs);
                        await context.SaveChangesAsync();
                        logger.LogInformation("Cleaned up {Count} old audit logs", oldLogs.Count);
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundJobs>>();
                    logger.LogError(ex, "Error cleaning up audit logs");
                    throw;
                }
            }
        }

        /// <summary>
        /// Generate and send daily attendance report
        /// </summary>
        [AutomaticRetry(Attempts = 3)]
        public static async Task GenerateDailyAttendanceReportAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<PunchDbContext>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundJobs>>();

                    var yesterday = DateTime.Today.AddDays(-1);

                    var departments = await context.Employees
                        .Where(e => e.IsActive)
                        .Select(e => e.Department)
                        .Distinct()
                        .ToListAsync();

                    foreach (var department in departments)
                    {
                        var employees = await context.Employees
                            .Where(e => e.Department == department && e.IsActive)
                            .ToListAsync();

                        var sb = new System.Text.StringBuilder();
                        sb.AppendLine($"Daily Attendance Report - {department}");
                        sb.AppendLine($"Date: {yesterday:yyyy-MM-dd}");
                        sb.AppendLine();
                        sb.AppendLine("Employee,Status,Hours Worked");

                        foreach (var emp in employees)
                        {
                            var records = await context.PunchRecords
                                .Where(p => p.EmployeeId == emp.Id && p.ActionDateTime.Date == yesterday)
                                .OrderBy(p => p.ActionDateTime)
                                .ToListAsync();

                            var status = records.Any() ? "Present" : "Absent";
                            var hoursWorked = CalculateHours(records);

                            sb.AppendLine($"{emp.FullName},{status},{hoursWorked:F2}");
                        }

                        logger.LogInformation("Generated attendance report for department: {Department}", department);
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<HangfireBackgroundJobs>>();
                    logger.LogError(ex, "Error generating daily attendance report");
                    throw;
                }
            }
        }

        private static decimal CalculateHours(List<Models.PunchRecord> records)
        {
            decimal totalHours = 0;

            for (int i = 0; i < records.Count - 1; i += 2)
            {
                if (records[i].ActionType == "PunchIn" && i + 1 < records.Count && records[i + 1].ActionType == "PunchOut")
                {
                    var duration = records[i + 1].ActionDateTime - records[i].ActionDateTime;
                    totalHours += (decimal)duration.TotalHours;
                }
            }

            return totalHours;
        }
    }
}