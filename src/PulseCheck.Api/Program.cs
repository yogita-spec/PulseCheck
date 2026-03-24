
using Microsoft.EntityFrameworkCore;
using PulseCheck.Api.Data;
using PulseCheck.Api.Services;
using PulseCheck.Api.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Register Controllers (like enabling MVC in old Global.asax)
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext> (options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<HealthCheckService>();
builder.Services.AddHostedService<BackgroundHealthChecker>();
// Register Swagger for API testing

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowReactApp");

// Show Swagger only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Tell the app to route requests to Controllers
app.MapControllers();

app.Run();