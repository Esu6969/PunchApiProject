using System;

namespace PunchApiProject.DTOs
{
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}