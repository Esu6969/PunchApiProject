namespace PunchApiProject.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}