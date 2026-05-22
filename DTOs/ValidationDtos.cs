using System.ComponentModel.DataAnnotations;

namespace PunchApiProject.DTOs
{
    /// <summary>
    /// Base response DTO for all API endpoints
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Employee registration DTO with comprehensive validation
    /// </summary>
    public class EmployeeRegistrationDto
    {
        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Employee ID must be between 3 and 50 characters")]
        [RegularExpression(@"^[A-Za-z0-9-_]+$", ErrorMessage = "Employee ID can only contain letters, numbers, dashes, and underscores")]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
            ErrorMessage = "Password must contain uppercase, lowercase, digit, and special character")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public string? JoinDate { get; set; }

        [Phone(ErrorMessage = "Each phone number must be in valid format")]
        public List<string> Phones { get; set; } = new();
    }

    /// <summary>
    /// Employee login DTO
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Punch request DTO by Employee ID
    /// </summary>
    public class PunchByEmployeeIdDto
    {
        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Punch request DTO by Database ID
    /// </summary>
    public class PunchRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Valid Employee ID is required")]
        public int EmployeeId { get; set; }

        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Employee update DTO
    /// </summary>
    public class EmployeeUpdateDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? FirstName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string? LastName { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string? Department { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string? Position { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Punch record response DTO
    /// </summary>
    public class PunchRecordDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime ActionDateTime { get; set; }
        public string ActionType { get; set; } = string.Empty;
    }

    /// <summary>
    /// Employee statistics response DTO
    /// </summary>
    public class EmployeeStatsDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal TodayHours { get; set; }
        public decimal WeekHours { get; set; }
        public decimal MonthHours { get; set; }
        public int TodayPunches { get; set; }
        public DateTime? LastPunchTime { get; set; }
        public string LastPunchType { get; set; } = string.Empty;
    }
}