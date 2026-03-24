# PulseCheck — Dev Notes (Yogita's Step-by-Step Log)

A day-by-day record of everything we built, every command we ran, and why.

---

## Session 1 — 2026-03-12 (Environment Setup)

### What we did
- Installed .NET 8 SDK, Node.js, Git, VS Code
- Created the solution file

### Commands run
```bash
dotnet new sln -n PulseCheck
```

### What it means
- `dotnet new sln -n PulseCheck` → creates an empty solution file (PulseCheck.sln). Like a container that holds all projects. Same as a blank Visual Studio solution.

---

## Session 2 — 2026-03-13 (Web API Project + First Controller)

### What we did
1. Created the Web API project
2. Added it to the solution
3. Explored the generated code
4. Cleaned up Program.cs (removed WeatherForecast sample)
5. Created the Controllers folder
6. Created our first real controller — EndpointsController

### Commands run
```bash
# Create the Web API project inside src/PulseCheck.Api folder
dotnet new webapi -n PulseCheck.Api -o src/PulseCheck.Api

# Register the project in the solution file
dotnet sln add src/PulseCheck.Api/PulseCheck.Api.csproj

# Run the API
dotnet run --project src/PulseCheck.Api

# Create Controllers folder
mkdir src/PulseCheck.Api/Controllers
```

### Key concepts learned

**Solution vs Project**
- `PulseCheck.sln` = empty container (like a TFS folder structure)
- `PulseCheck.Api.csproj` = actual project with runnable code
- `dotnet sln add` = registers the project inside the solution (Visual Studio does this automatically when you click OK on New Project dialog)

**Program.cs — the two phases**
```csharp
var builder = WebApplication.CreateBuilder(args);  // Construction phase — register services
builder.Services.AddControllers();                 // "I want to use Controllers"
builder.Services.AddEndpointsApiExplorer();        // Swagger: discover all endpoints
builder.Services.AddSwaggerGen();                  // Swagger: build the UI from those endpoints

var app = builder.Build();                         // Construction done, get the running app

app.UseSwagger();                                  // Turn on Swagger
app.UseSwaggerUI();                                // Turn on Swagger UI page
app.UseHttpsRedirection();                         // Redirect HTTP to HTTPS
app.MapControllers();                              // Route requests to Controllers

app.Run();                                         // Start the app
```

**Swagger**
- Auto-generated webpage at `/swagger` when app runs
- Shows all your endpoints and lets you test them with button clicks
- Like a test kitchen — you can place test orders before real customers arrive
- Registered once in Program.cs — automatically picks up all controllers

**Controller basics**
```csharp
[ApiController]                      // marks this as API controller
[Route("api/[controller]")]          // URL = /api/endpoints ([controller] = class name minus "Controller")
public class EndpointsController : ControllerBase   // ControllerBase = no Views, data only
{
    [HttpGet]                        // responds to GET requests
    public IActionResult Get()
    {
        return Ok("data here");      // returns 200 with data
    }
}
```

**ControllerBase vs Controller**
| Controller | ControllerBase |
|---|---|
| Old ASP.NET MVC — returns HTML Views | Web API — returns data (JSON) only |
| Has `return View()` | No View support |

### Files created/modified
- `src/PulseCheck.Api/` — entire Web API project
- `src/PulseCheck.Api/Program.cs` — cleaned up, Controllers enabled
- `src/PulseCheck.Api/Controllers/EndpointsController.cs` — first real endpoint

### Where we stopped
- `GET /api/endpoints` is live and returning a test message
- Next: create the `Endpoint` model (C# class representing a URL to monitor)

---

## Session 2 continued — 2026-03-13 (Models + EF Core setup)

### What we did
1. Created `MonitoredEndpoint` model (renamed from `Endpoint` to avoid conflict with built-in .NET class)
2. Updated `EndpointsController` to use the model with GET and POST
3. Tested GET and POST in Swagger successfully
4. Installed EF Core NuGet packages
5. Created `AppDbContext` — the bridge between C# and the database

### Commands run
```bash
# Create Models folder
mkdir src/PulseCheck.Api/Models

# Create Data folder
mkdir src/PulseCheck.Api/Data

# Install EF Core packages (must specify version 8.0.0 — matches our .NET 8 project)
# Without --version it grabs the latest (10.x) which is incompatible!
dotnet add src/PulseCheck.Api package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
dotnet add src/PulseCheck.Api package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

### Key concepts learned

**Model**
- Just a plain C# class with properties — represents real-world data
- `MonitoredEndpoint` = one URL that PulseCheck monitors
- No SQL needed — EF Core reads the class and creates the table automatically

```csharp
public class MonitoredEndpoint
{
    public int Id { get; set; }            // auto-assigned ID (like IDENTITY in SQL)
    public string Name { get; set; }       // friendly name e.g. "Google"
    public string Url { get; set; }        // the URL to monitor
    public bool IsActive { get; set; }     // is monitoring on or off
    public DateTime CreatedAt { get; set; } // when was it added
}
```

**Naming conflict error**
- `Endpoint` clashes with `Microsoft.AspNetCore.Http.Endpoint` (built into .NET)
- Fix: rename your class to something unique — `MonitoredEndpoint`
- Error message: "ambiguous reference" = .NET found two classes with the same name

**ORM / EF Core**
- ORM = Object Relational Mapper — translates C# ↔ SQL automatically
- Old way: write raw SQL or stored procedures, map results manually
- New way: write C# only, EF Core writes the SQL for you
- Your C# class = database table. Each property = column.

**DbContext**
- The middleman between your C# code and the database
- Like `SqlConnection` in your old code, but much more powerful
- `DbSet<MonitoredEndpoint> MonitoredEndpoints` → creates/maps the `MonitoredEndpoints` table
- Connection string still lives in `appsettings.json` (same as old `web.config`)
- But you never write `new SqlConnection(...)` — EF Core manages it for you

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // This one line = CREATE TABLE MonitoredEndpoints (...)
    public DbSet<MonitoredEndpoint> MonitoredEndpoints { get; set; }
}
```

**NuGet version mismatch**
- Always check your project's .NET version before installing packages
- `dotnet new webapi` creates a .NET 8 project
- EF Core latest = version 10.x (needs .NET 10) — incompatible!
- Fix: always add `--version 8.0.0` when installing EF Core packages

### Files created/modified
- `src/PulseCheck.Api/Models/MonitoredEndpoint.cs` — the data model
- `src/PulseCheck.Api/Controllers/EndpointsController.cs` — updated with GET + POST
- `src/PulseCheck.Api/Data/AppDbContext.cs` — EF Core database context

### Where we stopped — pick up here next session
Next steps:
1. Add connection string to `appsettings.json`
2. Register `AppDbContext` in `Program.cs`
3. Run EF Core migration — this is where the magic happens (C# class → real database table!)
4. Update controller to use the real database instead of the in-memory list
