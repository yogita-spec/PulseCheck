using Microsoft.EntityFrameworkCore;
using PulseCheck.Api.Models;

namespace PulseCheck.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // This tells EF Core: create a table called MonitoredEndpoints
    public DbSet<MonitoredEndpoint> MonitoredEndpoints { get; set; }

    public DbSet<HealthCheckResult> HealthCheckResults { get; set; }
}
