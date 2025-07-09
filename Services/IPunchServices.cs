using PunchApiProject.Models;
using System;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        PunchRecord PunchIn(string employeeName);
        PunchRecord PunchOut(int id);
        List<PunchRecord> GetAllPunchRecords();
        List<PunchRecord> GetPunchesByDate(DateTime date);
        TimeSpan? CalculateTotalHours(int id);
    }
}
