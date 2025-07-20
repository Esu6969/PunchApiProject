using PunchApiProject.DTOs;

public interface IEmployeeService
{
    Task<List<EmployeeFullRecordDto>> GetAllEmployeeActivityAsync();
}
