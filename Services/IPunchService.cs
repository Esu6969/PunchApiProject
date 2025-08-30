using PunchApiProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        Task AddPunchRecordAsync(Punch punch); // New method
        Task<IEnumerable<Punch>> GetAllPunchRecordsAsync();
        Task<IEnumerable<Punch>> FilterPunchRecordsByDateAsync(DateTime startDate, DateTime endDate);
        Task<TimeSpan> CalculateTotalHoursAsync(int employeeId);
    }
}