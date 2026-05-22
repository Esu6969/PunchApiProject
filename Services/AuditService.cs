using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Models;
using PunchApiProject.Services.Interfaces;

namespace PunchApiProject.Services
{
    public class AuditService : IAuditService
    {
        private readonly PunchDbContext _context;
        private readonly ILogger<AuditService> _logger;

        public AuditService(PunchDbContext context, ILogger<AuditService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Log an audit action
        /// </summary>
        public async Task LogActionAsync(int employeeId, string actionType, string action, string details = "", string ipAddress = "")
        {
            try
            {
                var auditLog = new AuditLog
                {
                    EmployeeId = employeeId,
                    ActionType = actionType,
                    Action = action,
                    Details = details,
                    IpAddress = ipAddress,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging audit action for employee {EmployeeId}", employeeId);
            }
        }

        /// <summary>
        /// Get audit logs for an employee
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(int employeeId, int days = 30)
        {
            try
            {
                var startDate = DateTime.UtcNow.AddDays(-days);

                return await _context.AuditLogs
                    .Where(a => a.EmployeeId == employeeId && a.CreatedAt >= startDate)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs for employee {EmployeeId}", employeeId);
                return Enumerable.Empty<AuditLog>();
            }
        }
    }
}