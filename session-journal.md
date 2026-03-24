# Session Journal

## Session 1 — 2026-03-12

Completed: Dev environment setup (.NET 8, Node, Git, VS Code), solution file created, learned REST APIs + HTTP status codes
Next up: Create the Web API project with `dotnet new webapi`, then create the folder structure
Stuck on: Nothing — smooth session

## Session 2 — 2026-03-13

Completed: Created Web API project, Controllers + Models + Data folders, MonitoredEndpoint model, EndpointsController with GET+POST, installed EF Core 8.0.0, created AppDbContext
Next up: Add connection string to appsettings.json, register DbContext in Program.cs, run first EF Core migration (C# → real database table!)
Stuck on: Nothing major — good instincts (caught naming conflict, NuGet version mismatch)

## Session 3 — 2026-03-15

Completed: Wired EF Core to SQLite, EndpointsController reads/writes real DB, built HealthCheckResult model (foreign key!), second migration, created HealthCheckService with HttpClient. Installed DB Browser for SQLite.
Next up: Register HealthCheckService in Program.cs, then build the BackgroundHealthChecker (the chowkidar)
Stuck on: Nothing — smooth session, excellent questions throughout

## Session 4 — 2026-03-15 (second session)

Completed: Registered HealthCheckService in DI, built BackgroundHealthChecker, registered as hosted service, ran app — chowkidar pinged yogita.com successfully
Next up: Recap BackgroundHealthChecker concepts (she wants a review), walk through each method in all files explaining what they do, then add a real URL via Swagger and watch it work. After that — build the API to get check history (/api/endpoints/{id}/history)
Stuck on: Needed deeper explanation of BackgroundService, stoppingToken, and CreateScope — all three clicked after walkthrough. Also requested method-by-method explanations for interview prep.

## Session 5 — 2026-03-18

Completed: History API with Select projection, React project (Vite+TS), App.tsx fetching real API data, CORS fixed, dashboard showing endpoints with green/red dots
Next up: Fix dashboard to show real isUp status (fetch latest HealthCheckResult per endpoint) + initialize Git repo and make first commit
Stuck on: localhost vs IP issue with Vite (fixed via vite.config host:0.0.0.0), CORS error (fixed via AddCors in Program.cs)
Note: Start Git workflow from next session onwards (git init, .gitignore, first commit, GitHub remote)
