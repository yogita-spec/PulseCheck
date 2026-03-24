using PulseCheck.Api.Models;

namespace PulseCheck.Api.Services;

public class HealthCheckService
{
    private readonly HttpClient _httpClient;

    public HealthCheckService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthCheckResult> CheckAsync(MonitoredEndpoint endpoint)
    {
        var result = new HealthCheckResult
        {
            MonitoredEndpointId = endpoint.Id,
            CheckedAt = DateTime.UtcNow
        };

        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var response = await _httpClient.GetAsync(endpoint.Url);
            stopwatch.Stop();

            result.IsUp = response.IsSuccessStatusCode;
            result.StatusCode = (int)response.StatusCode;
            result.ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds;
        }
        catch
        {
            result.IsUp = false;
            result.StatusCode = 0;
            result.ResponseTimeMs = 0;
        }

        return result;
    }
}
