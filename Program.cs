using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Load configuration from environment variables
builder.Configuration.AddEnvironmentVariables();


// Configure CORS for frontend
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

// Configure PostgreSQL Database - Direct connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Npgsql to handle DateTime properly
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContext<PunchDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Employee Punch API",
        Version = "v1",
        Description = "API for Employee Time Tracking System"
    });
});

// Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
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

// Enable static files
app.UseStaticFiles();

// Enable default files
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "login.html", "index.html" }
});

// Use CORS
app.UseCors("AllowFrontend");

// Enable Session
app.UseSession();

// Enable routing
app.UseRouting();

// Enable Authorization
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}));

// Fallback route for SPA
app.MapFallbackToFile("login.html");

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Employee Punch Tracking System Started");
logger.LogInformation("📍 Environment: {Environment}", app.Environment.EnvironmentName);
logger.LogInformation("🌐 Swagger UI: http://localhost:5031/swagger");
logger.LogInformation("🔐 Login Page: http://localhost:5031/login.html");

// Create database and tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    try
    {
        var punchDb = scope.ServiceProvider.GetRequiredService<PunchDbContext>();
        
        // This will create the database and tables if they don't exist
        punchDb.Database.EnsureCreated();
        
        logger.LogInformation("✅ Database and tables created successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error creating database tables: {Message}", ex.Message);
        logger.LogWarning("⚠️  Application will continue without database. Fix connection and restart.");
    }
}

app.Run();