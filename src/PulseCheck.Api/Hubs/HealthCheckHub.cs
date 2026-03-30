using Microsoft.AspNetCore.SignalR;

namespace PulseCheck.Api.Hubs;

// A SignalR Hub — think of it as a "conference call room"
// All connected browsers join this room automatically
// The server can broadcast messages to everyone in the room
public class HealthCheckHub : Hub
{
    // Broadcasts health check results to all connected browsers
    public async Task SendHealthCheckUpdate(object results)
    {
        await Clients.All.SendAsync("ListenForPing", results);
    }
}
