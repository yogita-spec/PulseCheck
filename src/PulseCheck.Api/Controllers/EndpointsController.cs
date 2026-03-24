using Microsoft.AspNetCore.Mvc;
using PulseCheck.Api.Models;
using PulseCheck.Api.Data;

namespace PulseCheck.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EndpointsController : ControllerBase
{
    private readonly AppDbContext _context;
    public EndpointsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Read all rows from MonitoredEndpoints table
        var endpoints = _context.MonitoredEndpoints.ToList();
        return Ok(endpoints);
    }

    [HttpPost]
    public IActionResult Create(MonitoredEndpoint endpoint)
    {
        // Add the new endpoint to the table and save
        _context.MonitoredEndpoints.Add(endpoint);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = endpoint.Id }, endpoint);
    }

    [HttpGet("{id}/history")]
    public IActionResult GetHistory(int id)
    {
        // Find the endpoint first — does it even exist?
        var endpoint = _context.MonitoredEndpoints.Find(id);
        if (endpoint == null)
            return NotFound();

        // Fetch all health check results for this endpoint, newest first
       var results = _context.HealthCheckResults
        .Where(r => r.MonitoredEndpointId == id)
        .OrderByDescending(r => r.CheckedAt)
        .Select(r => new
        {
            r.Id,
            r.IsUp,
            r.StatusCode,
            r.ResponseTimeMs,
            r.CheckedAt
        })
        .ToList();

        return Ok(results);
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        var result = _context.MonitoredEndpoints
            .Select(e => new
            {
                e.Id,
                e.Name,
                e.Url,
                e.IsActive,
                LatestCheck = _context.HealthCheckResults
                    .Where(h => h.MonitoredEndpointId == e.Id)
                    .OrderByDescending(h => h.CheckedAt)
                    .Select(h => new
                    {
                        h.IsUp,
                        h.StatusCode,
                        h.ResponseTimeMs,
                        h.CheckedAt
                    })
                    .FirstOrDefault()
            })
            .ToList();

        return Ok(result);
    }


}

