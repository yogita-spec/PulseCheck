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

## Session 6 — 2026-03-24

**Built:** Git init + first commit, status API with real isUp, styled dashboard cards, Add Endpoint form, React Router, EndpointDetail page with history
**New concepts:** Git init/add/commit, N+1 problem, React Router, useParams, SPA, template literals, optional chaining (?.)
**Milestone:** 13 of 18 features done — ~72% of MVP complete
**Identity note:** "Built a multi-page full-stack app with routing, forms, and real data."

## Session 7 — 2026-03-25 (morning, with Aniket)

**Built:** Git/GitHub reconciled, .gitignore updated, CLAUDE.md updated
**New concepts:** .gitignore rules, git remote, force push safety
**Milestone:** 13 of 18 — housekeeping session, no new features
**Identity note:** "Professional repo setup — clean history, no AI files in public repo."

## Session 8 — 2026-03-25 (evening)

**Built:** Fixed git commit messages (interactive rebase + force push), response time chart (Recharts), GET /api/endpoints/{id}, DELETE endpoint (API + UI), Edit endpoint (API + UI)
**New concepts:** git rebase -i, git stash/pop, git push --force, Recharts LineChart, hard vs soft delete, cascade delete via FK, useEffect vs event handlers, fetch with method/headers/body
**Milestone:** 15 of 18 features done — ~83% of MVP complete
**Identity note:** "Built a complete CRUD app with live charts. You can add, view, edit and delete monitored endpoints end-to-end."

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
- [x] Dashboard page (green/red dots) — real isUp status working
- [x] Add endpoint form
- [x] Endpoint detail page with history
- [x] Connect React to .NET API
- [x] Response time chart on detail page
- [x] Edit/Delete endpoint
- [x] SignalR integration

### Week 5-6: DevOps + Polish

- [ ] Dockerfile
- [ ] docker-compose.yml
- [ ] GitHub Actions CI pipeline
- [ ] Deploy to Azure App Service
- [ ] README with screenshots
- [ ] Unit tests (5-10)
- [ ] Open-source: LICENSE, CONTRIBUTING.md

## Session 9 — 2026-03-28

**Built:** SignalR real-time dashboard (Hub, IHubContext in BackgroundHealthChecker, React SignalR client), fixed stale dashboard data with useLocation, interview questions doc (60 questions)
**New concepts:** SignalR, Hub, IHubContext, WebSocket, CORS credentials, AllowCredentials, useLocation, useRef, HubConnectionBuilder, withAutomaticReconnect, Docker intro (tiffin box analogy)
**Milestone:** 16 of 22 features done — ~85% of MVP complete
**Identity note:** "Built a real-time dashboard that updates live without refreshing. That's production-grade monitoring software."
