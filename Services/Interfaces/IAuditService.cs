using PunchApiProject.Models;

namespace PunchApiProject.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogActionAsync(int employeeId, string actionType, string action, string details = "", string ipAddress = "");
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(int employeeId, int days = 30);
    }
}