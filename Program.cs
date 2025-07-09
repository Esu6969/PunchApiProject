using Microsoft.EntityFrameworkCore;
using PunchApiProject.Data;
using PunchApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=punch_data.db"));
builder.Services.AddScoped<IPunchService, PunchService>();

var app = builder.Build();

// Swagger + Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
