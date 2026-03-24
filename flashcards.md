# Flashcards — PulseCheck Learning Log

## REST API
**Q:** What is an API?
**A:** A waiter between two software systems. One system sends a request, the API fetches what's needed and sends back a response. You never talk directly to the kitchen.
**Metaphor:** Restaurant waiter — you order through the waiter, not by walking into the kitchen
**Date:** 2026-03-12

## HTTP Verbs
**Q:** What are the 4 main HTTP verbs and what do they mean?
**A:** GET = read/fetch, POST = create new, PUT = update existing, DELETE = remove. These are the "rules" of how a REST API waiter behaves.
**Metaphor:** Restaurant order actions — show menu (GET), place order (POST), change order (PUT), cancel order (DELETE)
**Date:** 2026-03-12

## HTTP Status Codes
**Q:** What do 200, 404, and 500 mean?
**A:** 200 = success (sab theek hai), 404 = not found (ghar pe koi nahi), 500 = server crashed internally (andar kuch toot gaya). 201 = new item created successfully.
**Metaphor:** Waiter reporting back how your order went
**Date:** 2026-03-12

## AllowedHosts in appsettings.json
**Q:** What does `"AllowedHosts": "*"` mean?
**A:** A security setting that controls which domain names can talk to your API. `*` means accept from anywhere — fine for local dev. In production you'd list only your real domain names to prevent Host Header attacks.
**Metaphor:** A guest list at the door — `*` means everyone is welcome, but in production you'd only let in specific names.
**Date:** 2026-03-15

## SQLite
**Q:** What is SQLite and how is it different from SQL Server?
**A:** SQLite is a database that lives entirely in a single `.db` file on your disk — no separate service or installation needed. SQL Server runs as a Windows service and is better for production with many users. SQLite is perfect for local dev and testing.
**Metaphor:** SQLite is like a tiffin box — self-contained, portable, everything inside. SQL Server is like a full restaurant kitchen — powerful but needs setup.
**Date:** 2026-03-15

## EF Core Migrations — two commands
**Q:** What is the difference between `migrations add` and `database update`?
**A:** `migrations add` generates the blueprint (a C# file with SQL instructions). `database update` executes that blueprint — actually creates the database file and tables. The word "update" means "bring the database up to date with the latest migration", not "update existing data".
**Metaphor:** `migrations add` = architect draws the blueprint. `database update` = construction crew builds the actual building.
**Date:** 2026-03-15

## Connection String Name Must Match
**Q:** Why must `"DefaultConnection"` in appsettings.json match `GetConnectionString("DefaultConnection")` in Program.cs?
**A:** It's a key-value lookup. appsettings.json stores the value under that key name. Program.cs fetches it using the same key. If the names don't match, .NET can't find the connection string and returns null.
**Metaphor:** Like a dictionary — if you store something under key "DefaultConnection", you must look it up using the same key.
**Date:** 2026-03-15

## BackgroundService (IHostedService)
**Q:** What is a BackgroundService in .NET?
**A:** A class that runs in the background of your API, on a loop, without waiting for any HTTP request. It starts when your app starts and keeps running. Like a SQL Agent job, but written in C# and living inside your API instead of inside SQL Server.
**Metaphor:** A chowkidar (watchman) doing rounds — he checks every door every 30 minutes, doesn't wait for anyone to ask him. SQL Agent jobs do the same but inside SQL Server; BackgroundService does the same inside your .NET app.
**Date:** 2026-03-15

## AddHostedService<T>()
**Q:** What does `builder.Services.AddHostedService<T>()` do in Program.cs?
**A:** It registers a background service with .NET's hosting system. Unlike regular services that wait to be called, a hosted service starts running **automatically** when the app starts. .NET calls its `ExecuteAsync` method immediately on startup.
**Metaphor:** Like hiring a chowkidar — the moment the building opens, he starts his rounds. You don't need to call him, he just starts.
**Date:** 2026-03-15

## AddHttpClient<T>()
**Q:** What does `builder.Services.AddHttpClient<T>()` do?
**A:** It registers a service AND gives it a managed `HttpClient` for making HTTP calls. .NET handles creating, reusing, and disposing the HttpClient properly (avoids socket exhaustion). You use this when your service needs to call external URLs.
**Metaphor:** Like registering a delivery person and also giving them a vehicle — the service is registered AND equipped with the tool it needs (HttpClient).
**Date:** 2026-03-15

## CancellationToken / stoppingToken
**Q:** What is `CancellationToken stoppingToken` in a BackgroundService?
**A:** A kill switch provided by .NET. When the app shuts down (Ctrl+C or server restart), .NET sets `stoppingToken.IsCancellationRequested = true`. The background loop checks this and exits gracefully. Without it, the loop would run forever and the app could never shut down cleanly.
**Metaphor:** A walkie-talkie from the building manager — when he says "shift over", the chowkidar finishes his current round and goes home.
**Date:** 2026-03-15

## IServiceProvider + CreateScope (Scoped vs Singleton lifetime)
**Q:** Why does BackgroundHealthChecker use `IServiceProvider.CreateScope()` instead of injecting `AppDbContext` directly?
**A:** A controller is short-lived (created per request, disposed after). A BackgroundService lives forever. `AppDbContext` is designed to be short-lived — if the chowkidar held the same one forever, the DB connection goes stale and memory piles up. So every 30 seconds, he borrows a fresh `AppDbContext` via `CreateScope()`, uses it, and returns it (`using` disposes it).
**Metaphor:** Controller = paper cup (use once, throw away). Chowkidar works 24/7 — can't keep one paper cup forever. He borrows a fresh cup from the kitchen (`IServiceProvider`) each round and returns it when done.
**Date:** 2026-03-15

## ExecuteAsync (BackgroundService method)
**Q:** What is `ExecuteAsync` in a BackgroundService?
**A:** The one method you must override. .NET calls it automatically when the app starts. Whatever you put inside runs in the background. Typically contains a `while` loop that does work, sleeps, and repeats. You never call this method yourself — .NET does.
**Metaphor:** The chowkidar's job description — "do rounds, check doors, repeat." You write the instructions, .NET tells him to start.
**Date:** 2026-03-15

## Key Methods in Our Code (Interview Reference)

### Program.cs — Service Registration Methods
**Q:** Explain each `builder.Services.Add...` line in Program.cs.
**A:**
- `AddControllers()` — Registers MVC controllers so .NET can route HTTP requests to them (like enabling MVC in old Global.asax)
- `AddDbContext<AppDbContext>(options => ...)` — Registers the EF Core database context with a connection string. Creates a fresh one per request.
- `AddHttpClient<HealthCheckService>()` — Registers HealthCheckService and gives it an HttpClient for making HTTP calls
- `AddHostedService<BackgroundHealthChecker>()` — Registers a background worker that starts automatically with the app
- `AddEndpointsApiExplorer()` + `AddSwaggerGen()` — Enables Swagger UI for testing your API in the browser
**Date:** 2026-03-15

### EndpointsController — Action Methods
**Q:** What do the methods in EndpointsController do?
**A:**
- `GetAll()` [GET /api/endpoints] — Returns all monitored endpoints from the database using `_db.MonitoredEndpoints.ToListAsync()`. Returns `Ok(endpoints)` which sends a 200 response with JSON data.
- `Create(endpoint)` [POST /api/endpoints] — Receives a new endpoint from the request body, adds it to the database with `_db.MonitoredEndpoints.Add()`, saves with `SaveChangesAsync()`, and returns `Created()` (201 status).
**Date:** 2026-03-15

### HealthCheckService — CheckAsync Method
**Q:** What does `CheckAsync(MonitoredEndpoint endpoint)` do?
**A:** Takes a monitored endpoint, pings its URL using `HttpClient.GetAsync()`, measures response time with `Stopwatch`, and returns a `HealthCheckResult` with: IsUp (success/fail), StatusCode (200/404/500 etc), ResponseTimeMs (how fast). If the request throws an exception (URL doesn't exist, timeout), it catches it and marks IsUp = false.
**Date:** 2026-03-15

### BackgroundHealthChecker — Key Methods
**Q:** Explain the methods in BackgroundHealthChecker.
**A:**
- `ExecuteAsync(stoppingToken)` — Called by .NET on app startup. Contains the `while` loop: check all endpoints → sleep 30 seconds → repeat. Stops when `stoppingToken` is cancelled (app shutting down).
- `CheckAllEndpointsAsync()` — One round of checking. Creates a fresh DB scope, fetches all endpoints, pings each one using `HealthCheckService.CheckAsync()`, saves all results to DB. This is one "round" of the chowkidar.
**Date:** 2026-03-15

## dotnet-ef is a separate tool
**Q:** Why does `dotnet ef` need to be installed separately?
**A:** `dotnet` is the base toolbox that comes with .NET. `dotnet ef` is a special add-on tool for EF Core migrations — like a drill bit you attach separately. Install it once globally with `dotnet tool install --global dotnet-ef`. The tool version should match your EF Core version (e.g. v8 for EF Core 8).
**Metaphor:** Like installing a VS Code extension — the base editor is there, but some features need to be added separately.
**Date:** 2026-03-15

