// Middleware/SessionMiddleware.cs
using Microsoft.AspNetCore.Http;

namespace PunchApiProject.Middleware
{
    /// <summary>
    /// Extension methods for session management
    /// </summary>
    public static class SessionExtensions
    {
        private const string SESSION_EMPLOYEE_ID = "EmployeeId";
        private const string SESSION_EMPLOYEE_NAME = "EmployeeName";
        private const string SESSION_DEPARTMENT = "Department";
        private const string SESSION_POSITION = "Position";
        private const string SESSION_ID = "Id";

        /// <summary>
        /// Set login session for employee
        /// </summary>
        public static void SetLoginSession(this HttpContext httpContext, string employeeId, string employeeName, string department, string position, int id)
        {
            httpContext.Session.SetString(SESSION_EMPLOYEE_ID, employeeId);
            httpContext.Session.SetString(SESSION_EMPLOYEE_NAME, employeeName);
            httpContext.Session.SetString(SESSION_DEPARTMENT, department);
            httpContext.Session.SetString(SESSION_POSITION, position);
            httpContext.Session.SetInt32(SESSION_ID, id);
        }

        /// <summary>
        /// Clear login session
        /// </summary>
        public static void ClearLoginSession(this HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }

        /// <summary>
        /// Get Employee ID from session
        /// </summary>
        public static string? GetSessionEmployeeId(this HttpContext httpContext)
        {
            return httpContext.Session.GetString(SESSION_EMPLOYEE_ID);
        }

        /// <summary>
        /// Get Employee Name from session
        /// </summary>
        public static string? GetSessionEmployeeName(this HttpContext httpContext)
        {
            return httpContext.Session.GetString(SESSION_EMPLOYEE_NAME);
        }

        /// <summary>
        /// Get Department from session
        /// </summary>
        public static string? GetSessionDepartment(this HttpContext httpContext)
        {
            return httpContext.Session.GetString(SESSION_DEPARTMENT);
        }

        /// <summary>
        /// Get Position from session
        /// </summary>
        public static string? GetSessionPosition(this HttpContext httpContext)
        {
            return httpContext.Session.GetString(SESSION_POSITION);
        }

        /// <summary>
        /// Get Employee ID (int) from session
        /// </summary>
        public static int? GetSessionId(this HttpContext httpContext)
        {
            return httpContext.Session.GetInt32(SESSION_ID);
        }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        public static bool IsLoggedIn(this HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.GetSessionEmployeeId());
        }
    }

    /// <summary>
    /// Middleware to validate session on protected routes
    /// </summary>
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SessionValidationMiddleware> _logger;

        public SessionValidationMiddleware(RequestDelegate next, ILogger<SessionValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // List of public routes that don't require authentication
            var publicRoutes = new[] { "/api/auth/login", "/api/auth/register", "/health" };

            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Check if route is public
            bool isPublicRoute = publicRoutes.Any(route => path.StartsWith(route));

            if (!isPublicRoute && !context.IsLoggedIn())
            {
                _logger.LogWarning("Unauthorized access attempt to {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { success = false, message = "Not authenticated" });
                return;
            }

            await _next(context);
        }
    }

    /// <summary>
    /// Extension to add session validation middleware
    /// </summary>
    public static class SessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionValidationMiddleware>();
        }
    }
}