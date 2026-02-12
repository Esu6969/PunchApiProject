// Program.cs

using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Services;
using PunchApiProject.Middleware; // ✅ import middleware namespace

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

// ✅ Application Services
builder.Services.AddScoped<IPunchService, PunchService>();

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
        Description = "API for Employee Time Tracking System"
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

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "login.html", "index.html" }
});

app.UseStaticFiles();           // 1️⃣ Serve static files first

app.UseRouting();               // 2️⃣ Routing

app.UseCors("AllowFrontend");   // 3️⃣ CORS

app.UseSession();               // 4️⃣ Session (must be before middleware)

app.UseSessionValidation();     // 5️⃣ ✅ Our custom session middleware
                                //    automatically checks session on all
                                //    protected routes — no manual checks needed

app.UseAuthorization();         // 6️⃣ Authorization

app.MapControllers();           // 7️⃣ Controllers

// Health check — public route (no session needed)
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}));

app.MapFallbackToFile("login.html");

// ── Startup Logs ─────────────────────────────────────────────
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application Started");
logger.LogInformation("Swagger: http://localhost:5031/swagger");
logger.LogInformation("Login:   http://localhost:5031/login.html");

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