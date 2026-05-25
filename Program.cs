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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ✅ Database
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
        Description = "Employee Time Tracking System - Backend API Only"
    });
});

var app = builder.Build();

// ── Middleware Pipeline (ORDER IS CRITICAL) ──────────────────

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Punch API V1");
        c.RoutePrefix = "swagger";
    });
    app.UseDeveloperExceptionPage();
    app.UseHangfireDashboard("/hangfire");
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseSession();
app.UseAuthorization();
app.MapControllers();

// ── Health Check ──────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
})).WithName("Health").WithOpenApi();

// ── Schedule Hangfire Jobs ────────────────────────────────────
RecurringJob.AddOrUpdate(
    "send-daily-reminders",
    () => HangfireBackgroundJobs.SendDailyPunchRemindersAsync(app.Services),
    Cron.Daily(9, 0));

RecurringJob.AddOrUpdate(
    "generate-daily-report",
    () => HangfireBackgroundJobs.GenerateDailyAttendanceReportAsync(app.Services),
    Cron.Daily(18, 0));

RecurringJob.AddOrUpdate(
    "cleanup-audit-logs",
    () => HangfireBackgroundJobs.CleanupOldAuditLogsAsync(app.Services),
    Cron.Daily(2, 0));

// ── Startup Logs ─────────────────────────────────────────────
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Employee Punch API Started");
logger.LogInformation("📊 Swagger: http://localhost:5031/swagger");
logger.LogInformation("🎯 Hangfire: http://localhost:5031/hangfire");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var punchDb = scope.ServiceProvider.GetRequiredService<PunchDbContext>();
        punchDb.Database.Migrate();
        logger.LogInformation("✅ Database ready");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Database error");
    }
}

app.Run();

