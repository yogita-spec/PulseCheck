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

## Session 6 — 2026-03-24

Completed: Git init + first commit + GitHub remote, status API with real isUp, styled dashboard, Add Endpoint form, React Router, EndpointDetail history page
Next up: Response time chart (Recharts), then SignalR
Stuck on: localhost vs IP Vite issue (recurring), API already running lock error (harmless)

## Session 7 — 2026-03-25 (morning, Aniket)

Completed: Git/GitHub reconciled, .gitignore updated (AI files excluded), CLAUDE.md updated
Next up: Fix em dash commit messages (rebase lesson), then response time chart
Stuck on: Nothing - housekeeping session

## Session 8 — 2026-03-25 (evening)

Completed: Git rebase to fix em dash commit messages + force push, Recharts response time chart, GET /api/endpoints/{id}, full Edit + Delete (API + React UI)
Next up: SignalR for real-time dashboard updates — the "wow" feature
Stuck on: Vim during rebase (switched to GitLens UI), Windows VPN popup (unrelated)

## Session 9 — 2026-03-28

Completed: SignalR end-to-end (Hub + BackgroundHealthChecker broadcast + React client), fixed stale data bug with useLocation, interview questions doc (60 Qs)
Next up: Docker — Dockerfile + docker-compose (concept introduced, will build next session)
Stuck on: URL mismatch ("/hub" vs "/hubs") — good debugging lesson. Also requested to type all code herself (feedback saved).
