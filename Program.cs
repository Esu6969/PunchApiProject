// Program.cs

using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Services;
using PunchApiProject.Services.Interfaces;
using PunchApiProject.Middleware;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// ── Services ────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:8000",
            "http://127.0.0.1:8000",
            "http://localhost:5500",
            "http://127.0.0.1:5500",
            "http://localhost:3000",
            "http://127.0.0.1:3000"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// ✅ Database
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<PunchDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Hangfire Configuration
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

// ✅ Application Services
builder.Services.AddScoped<IPunchService, PunchService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IReportService, ReportService>();

// ✅ Session — 30 minute timeout
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".PunchApp.Session";
});

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Employee Punch API",
        Version = "v1",
        Description = "API for Employee Time Tracking System with Hangfire Background Jobs"
    });
});

var app = builder.Build();

// ── Middleware Pipeline (ORDER IS CRITICAL) ──────────────────

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Punch API V1");
        c.RoutePrefix = "swagger";
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// ✅ Hangfire Dashboard (optional - only in development)
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        DashboardTitle = "Punch API Background Jobs",
        Authorization = new[] { new MyAuthorizationFilter() }
    });
}

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "login.html", "index.html" }
});

app.UseStaticFiles();           // 1️⃣ Serve static files first
app.UseRouting();               // 2️⃣ Routing
app.UseCors("AllowFrontend");   // 3️⃣ CORS
app.UseSession();               // 4️⃣ Session
app.UseSessionValidation();     // 5️⃣ Custom session middleware
app.UseAuthorization();         // 6️⃣ Authorization
app.MapControllers();           // 7️⃣ Controllers

// ── Health Check ──────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}));

app.MapFallbackToFile("login.html");

// ── Schedule Hangfire Jobs ────────────────────────────────────
RecurringJob.AddOrUpdate(
    "send-daily-reminders",
    () => HangfireBackgroundJobs.SendDailyPunchRemindersAsync(app.Services),
    Cron.Daily(9, 0)); // 9 AM every day

RecurringJob.AddOrUpdate(
    "generate-daily-report",
    () => HangfireBackgroundJobs.GenerateDailyAttendanceReportAsync(app.Services),
    Cron.Daily(18, 0)); // 6 PM every day

RecurringJob.AddOrUpdate(
    "cleanup-audit-logs",
    () => HangfireBackgroundJobs.CleanupOldAuditLogsAsync(app.Services),
    Cron.Daily(2, 0)); // 2 AM every day

// ── Startup Logs ─────────────────────────────────────────────
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application Started");
logger.LogInformation("Swagger: http://localhost:5031/swagger");
logger.LogInformation("Login:   http://localhost:5031/login.html");
logger.LogInformation("Hangfire Dashboard: http://localhost:5031/hangfire");

// ── Database Setup ────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    try
    {
        var punchDb = scope.ServiceProvider.GetRequiredService<PunchDbContext>();
        punchDb.Database.EnsureCreated();
        logger.LogInformation("Database ready");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database error: {Message}", ex.Message);
        logger.LogWarning("Application started without database. Fix connection and restart.");
    }
}

app.Run();

// ── Hangfire Authorization Filter ────────────────────────────
public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // In production, implement proper authentication
        var httpContext = context.GetHttpContext();
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}