# PulseCheck — MVP Progress Tracker

## Session 1 — 2026-03-12

**Built:** Development environment setup, solution file created (PulseCheck.sln)
**New concepts:** REST APIs, HTTP verbs (GET/POST/PUT/DELETE), HTTP status codes (200/404/500)
**Milestone:** 0 of 18 features done — ~5% of MVP complete (environment + foundation ready)
**Identity note:** "Set up a professional .NET 8 dev environment from scratch — ready to build."

## Session 2 — 2026-03-13

**Built:** Web API project, MonitoredEndpoint model, EndpointsController (GET + POST), AppDbContext, EF Core installed
**New concepts:** Solution vs project, Program.cs pipeline, Swagger, Controllers, Models, ORM, DbContext, NuGet versioning
**Milestone:** 2 of 18 features done — ~15% of MVP complete
**Identity note:** "Built a working REST API with GET and POST endpoints — you're a backend developer now."

## Session 3 — 2026-03-15

**Built:** EF Core + SQLite wired up, EndpointsController uses real DB, HealthCheckResult model with FK, second migration, HealthCheckService created
**New concepts:** SQLite, EF Core migrations, dotnet-ef tool, DI in controllers, IActionResult, SaveChanges, DB Browser for SQLite, AllowedHosts, HttpClient, async/await Task, Stopwatch, foreign keys, navigation properties
**Milestone:** 5 of 18 features done — ~30% of MVP complete
**Identity note:** "Built a health check service that pings URLs and measures response times. You're building real monitoring software now."

## Session 4 — 2026-03-15 (second session today)

**Built:** Registered HealthCheckService in Program.cs, built BackgroundHealthChecker (the chowkidar), registered it as a hosted service, ran app and saw it automatically ping URLs
**New concepts:** BackgroundService base class, ExecuteAsync, CancellationToken/stoppingToken, IServiceProvider + CreateScope (scoped vs singleton lifetime), AddHostedService, AddHttpClient
**Milestone:** 6 of 18 features done — ~35% of MVP complete
**Identity note:** "Built a fully automated monitoring system — a chowkidar that pings URLs on its own. That's real infrastructure software."

## Session 5 — 2026-03-18

**Built:** History API (/api/endpoints/{id}/history) with Select projection, React project setup (Vite + TypeScript), App.tsx dashboard fetching real API data, CORS configured, endpoints showing on screen
**New concepts:** Route parameters, LINQ Select projection, anonymous objects, React useState, useEffect, JSX, .map(), fetch API, CORS, Vite config, npm install
**Milestone:** 9 of 18 features done — ~50% of MVP complete
**Identity note:** "Built a full-stack app — a React frontend talking to a .NET API showing live data. You're a full-stack developer now."

## MVP Feature Checklist

### Week 1-2: Backend Foundation

- [x] Project setup (.NET 8 Web API, EF Core, SQLite)
- [x] `MonitoredEndpoint` model
- [x] `HealthCheckResult` model
- [x] CRUD API for managing endpoints (`/api/endpoints`)
- [x] Basic health check service
- [x] `BackgroundHealthChecker` — IHostedService
- [x] API to get check history

### Week 3-4: Frontend + Real-Time

- [x] React project setup (Vite + TypeScript)
- [ ] Dashboard page (green/red dots) — in progress, needs real isUp status
- [ ] Add/Edit endpoint form
- [ ] Endpoint detail page with response time chart
- [ ] SignalR integration
- [ ] Connect React to .NET API

### Week 5-6: DevOps + Polish

- [ ] Dockerfile
- [ ] docker-compose.yml
- [ ] GitHub Actions CI pipeline
- [ ] Deploy to Azure App Service
- [ ] README with screenshots
- [ ] Unit tests (5-10)
- [ ] Open-source: LICENSE, CONTRIBUTING.md
