//using Microsoft.AspNetCore.Mvc;
//using PunchApiProject.DTOs;

//[ApiController]
//[Route("api/[controller]")]
//public class RegistrationController : ControllerBase
//{
//    private readonly punchDbContext _db;

//    public RegistrationController(punchDbContext db)
//    {
//        _db = db;
//    }

//    [HttpPost("register")]
//    public async Task<IActionResult> Register(RegisterDto dto)
//    {
//        var user = new Employee
//        {
//            Name = dto.Name,
//            Email = dto.Email,
//            Password = dto.Password
//        };

//        _db.Employees.Add(user);
//        await _db.SaveChangesAsync();

//        return Ok("Registered Successfully");
//    }
//}
