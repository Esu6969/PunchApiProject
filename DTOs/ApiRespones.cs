namespace PunchApiProject.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public int StatusCode { get; set; } = 200;
        public string? Error { get; set; }
    }
}