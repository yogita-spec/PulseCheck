namespace PulseCheck.Api.Models;

public class HealthCheckResult 
{
    public int Id { get; set; }
    public int MonitoredEndpointId  { get; set; }
    public bool IsUp  { get; set; } = true;
    public int  StatusCode {get;set;}
    public int ResponseTimeMs   { get; set; }
    public DateTime CheckedAt  { get; set; } = DateTime.UtcNow;

  // Navigation property — tells EF Core this is a foreign key relationship
    public MonitoredEndpoint Endpoint { get; set; } = null!;
}