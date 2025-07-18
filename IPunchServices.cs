using PunchApiProject.DTOs;
using PunchApiProject.Models;

namespace PunchApiProject.Services
{
    public interface IPunchService
    {
        Task<List<PunchRecord>> GetAllAsync();
        Task<PunchRecord?> GetByIdAsync(int id);
        Task<PunchRecord> PunchInAsync(CreatePunchRecordDto dto);
        Task<PunchRecord?> PunchOutAsync(UpdatePunchOutDto dto);
    }
}
