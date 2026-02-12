// Middleware/SessionMiddleware.cs

namespace PunchApiProject.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SessionMiddleware> _logger;

        // ✅ These routes are PUBLIC — no session needed
        private static readonly string[] PublicRoutes = new[]
        {
            "/api/auth/login",
            "/api/auth/register",
            "/health",
            "/swagger",
            "/login.html",
            "/index.html",
            "/favicon.ico"
        };

        public SessionMiddleware(RequestDelegate next, ILogger<SessionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // ✅ Step 1 — Check if route is public (no session needed)
            bool isPublicRoute = PublicRoutes.Any(route =>
                path.StartsWith(route.ToLower()));

            if (isPublicRoute)
            {
                // Public route — skip session check, go to next middleware
                await _next(context);
                return;
            }

            // ✅ Step 2 — Check if session exists for protected routes
            var employeeId = context.Session.GetString("EmployeeId");

            if (string.IsNullOrEmpty(employeeId))
            {
                _logger.LogWarning("Unauthorized access attempt to: {Path}", path);

                // Session not found — return 401
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "Session expired or not logged in. Please login again.",
                        redirectTo = "/login.html"
                    })
                );
                return;
            }

            // ✅ Step 3 — Session is valid, attach user info to HttpContext
            // This makes user info available in all controllers
            context.Items["EmployeeId"] = employeeId;
            context.Items["EmployeeName"] = context.Session.GetString("EmployeeName");
            context.Items["Department"] = context.Session.GetString("Department");
            context.Items["Position"] = context.Session.GetString("Position");
            context.Items["Id"] = context.Session.GetInt32("Id");

            _logger.LogInformation("Session valid for: {EmployeeId} accessing {Path}",
                employeeId, path);

            // ✅ Step 4 — Continue to next middleware / controller
            await _next(context);
        }
    }

    // ✅ Extension method — makes it easy to register in Program.cs
    public static class SessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SessionMiddleware>();
        }
    }
}