using PunchApiProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        Task PunchInAsync(int employeeId);
        Task PunchOutAsync(int employeeId);
        Task<IEnumerable<Punch>> GetAllPunchRecordsAsync();
        Task<IEnumerable<Punch>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate);
        Task<TimeSpan> CalculateTotalHoursAsync(int employeeId);
    }
}
