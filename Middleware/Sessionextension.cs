// Middleware/SessionExtensions.cs

namespace PunchApiProject.Middleware
{
    // ✅ Helper class — read session data easily inside any controller
    public static class SessionExtensions
    {
        // Get EmployeeId from session
        public static string? GetSessionEmployeeId(this HttpContext context)
        {
            return context.Session.GetString("EmployeeId");
        }

        // Get Employee Name from session
        public static string? GetSessionEmployeeName(this HttpContext context)
        {
            return context.Session.GetString("EmployeeName");
        }

        // Get Department from session
        public static string? GetSessionDepartment(this HttpContext context)
        {
            return context.Session.GetString("Department");
        }

        // Get Position from session
        public static string? GetSessionPosition(this HttpContext context)
        {
            return context.Session.GetString("Position");
        }

        // Get integer Id from session
        public static int? GetSessionId(this HttpContext context)
        {
            return context.Session.GetInt32("Id");
        }

        // Check if session is valid
        public static bool IsSessionValid(this HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Session.GetString("EmployeeId"));
        }

        // Set all session values at login
        public static void SetLoginSession(this HttpContext context,
            string employeeId,
            string employeeName,
            string department,
            string position,
            int id)
        {
            context.Session.SetString("EmployeeId", employeeId);
            context.Session.SetString("EmployeeName", employeeName);
            context.Session.SetString("Department", department);
            context.Session.SetString("Position", position);
            context.Session.SetInt32("Id", id);
        }

        // Clear session at logout
        public static void ClearLoginSession(this HttpContext context)
        {
            context.Session.Clear();
        }
    }
}