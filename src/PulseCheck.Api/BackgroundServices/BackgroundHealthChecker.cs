using Microsoft.EntityFrameworkCore;
using PulseCheck.Api.Data;
using PulseCheck.Api.Services;

namespace PulseCheck.Api.BackgroundServices;

public class BackgroundHealthChecker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HealthCheckService _healthCheckService;
    private readonly ILogger<BackgroundHealthChecker> _logger;

    public BackgroundHealthChecker(
        IServiceProvider serviceProvider,
        HealthCheckService healthCheckService,
        ILogger<BackgroundHealthChecker> logger)
    {
        _serviceProvider = serviceProvider;
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    // TODO: Next session — Yogita, tell Claude "let's recap the chowkidar"
    // We'll walk through this file again, add a real URL like https://google.com,
    // and then build the history API so you can see all past check results!
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BackgroundHealthChecker started — chowkidar is on duty!");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAllEndpointsAsync();
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task CheckAllEndpointsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var endpoints = await db.MonitoredEndpoints.ToListAsync();

        foreach (var endpoint in endpoints)
        {
            var result = await _healthCheckService.CheckAsync(endpoint);
            db.HealthCheckResults.Add(result);

            _logger.LogInformation(
                "{Url} — {Status} ({ResponseTime}ms)",
                endpoint.Url, result.IsUp ? "UP" : "DOWN", result.ResponseTimeMs);
        }

        await db.SaveChangesAsync();
    }
}
