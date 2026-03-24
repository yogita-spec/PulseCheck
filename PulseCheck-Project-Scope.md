# PulseCheck — Project Scope & Structure

## What Is PulseCheck?

PulseCheck is a **self-hostable API health monitoring tool**. Think of it like a lightweight, open-source version of UptimeRobot or Pingdom. Users register API endpoints, PulseCheck polls them on a schedule, and a real-time dashboard shows what's up, what's down, and response time trends.

**Why this project?**
- Genuinely useful — developers will actually use and star it
- Covers every skill employers want: REST API, background services, real-time (SignalR), database, React frontend, Docker, CI/CD
- Easy to explain in 30 seconds in an interview
- Natural room for growth (email alerts, webhook notifications, response time charts)

---

## Tech Stack

| Layer | Technology | Why |
|-------|-----------|-----|
| Backend API | ASP.NET Core 8 Web API | Modern .NET, high demand, natural fit for C# experience |
| Database | EF Core 8 + SQLite (dev) / SQL Server (prod) | ORM she'll recognize from ADO.NET/older EF, SQLite means zero setup locally |
| Real-time | SignalR | Push updates to dashboard without polling — impressive in demos |
| Background work | IHostedService | Polls registered endpoints on a timer — shows async/concurrency skills |
| Frontend | React 18 + TypeScript + Vite | Market-leading frontend, TypeScript feels like C# |
| Styling | Tailwind CSS | Fast, no design skill needed, looks professional |
| Testing | xUnit + Moq | Standard .NET testing — she may already know this |
| Containers | Docker + docker-compose | One command to run everything — critical resume keyword |
| CI/CD | GitHub Actions | Free, widely used, shows DevOps awareness |
| Deployment | Azure App Service (free tier) | Live demo URL for resume — Microsoft ecosystem fits her background |

---

## Features (MVP Scope — 6 Weeks)

### Week 1-2: Backend Foundation
- [ ] Project setup (.NET 8 Web API, EF Core, SQLite)
- [ ] `Endpoint` model: URL, name, check interval, expected status code
- [ ] `HealthCheckResult` model: timestamp, status code, response time, is healthy
- [ ] CRUD API for managing endpoints (`/api/endpoints`)
- [ ] Basic health check service that pings a URL and records the result
- [ ] `BackgroundHealthChecker` — IHostedService that runs checks on a timer
- [ ] API to get check history (`/api/endpoints/{id}/history`)

### Week 3-4: Frontend + Real-Time
- [ ] React project setup (Vite + TypeScript)
- [ ] Dashboard page showing all endpoints with status (green/red dots)
- [ ] Add/Edit endpoint form
- [ ] Endpoint detail page with response time chart (recharts)
- [ ] SignalR integration — dashboard updates live when a check completes
- [ ] Connect React to .NET API (axios or fetch)

### Week 5-6: DevOps + Polish
- [ ] Dockerfile for the API
- [ ] docker-compose.yml (API + SQL Server + React)
- [ ] GitHub Actions CI pipeline (build, test, lint)
- [ ] Deploy to Azure App Service (free tier)
- [ ] README with screenshots, setup instructions, architecture diagram
- [ ] Add 5-10 unit tests for critical paths
- [ ] Open-source: LICENSE (MIT), CONTRIBUTING.md

### Stretch Goals (Post-MVP)
- Email/Slack notifications on downtime
- Webhook alerts
- Multi-user with authentication (ASP.NET Identity)
- SSL certificate expiry monitoring
- Public status page (shareable URL)

---

## Folder Structure

```
PulseCheck/
├── CLAUDE.md                        ← Claude Code teaching instructions
├── flashcards.md                    ← Auto-generated learning log
├── session-journal.md               ← Session history (Claude reads on start)
├── progress.md                      ← MVP progress tracker with milestones
├── README.md                        ← GitHub project page
├── .gitignore
│
├── src/
│   ├── PulseCheck.Api/              ← ASP.NET Core 8 Web API
│   │   ├── Controllers/
│   │   │   ├── EndpointsController.cs
│   │   │   └── HealthChecksController.cs
│   │   ├── Models/
│   │   │   ├── MonitoredEndpoint.cs
│   │   │   └── HealthCheckResult.cs
│   │   ├── Data/
│   │   │   ├── PulseCheckDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Services/
│   │   │   ├── IHealthCheckService.cs
│   │   │   └── HealthCheckService.cs
│   │   ├── BackgroundServices/
│   │   │   └── HealthCheckWorker.cs
│   │   ├── Hubs/
│   │   │   └── DashboardHub.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── PulseCheck.Api.csproj
│   │
│   └── pulsecheck-ui/               ← React + TypeScript frontend
│       ├── src/
│       │   ├── components/
│       │   │   ├── Dashboard.tsx
│       │   │   ├── EndpointCard.tsx
│       │   │   ├── EndpointForm.tsx
│       │   │   ├── EndpointDetail.tsx
│       │   │   └── ResponseTimeChart.tsx
│       │   ├── hooks/
│       │   │   └── useSignalR.ts
│       │   ├── services/
│       │   │   └── api.ts
│       │   ├── types/
│       │   │   └── index.ts
│       │   ├── App.tsx
│       │   └── main.tsx
│       ├── index.html
│       ├── package.json
│       ├── tsconfig.json
│       ├── vite.config.ts
│       └── tailwind.config.js
│
├── tests/
│   └── PulseCheck.Api.Tests/
│       ├── Controllers/
│       │   └── EndpointsControllerTests.cs
│       ├── Services/
│       │   └── HealthCheckServiceTests.cs
│       └── PulseCheck.Api.Tests.csproj
│
├── Dockerfile
├── docker-compose.yml
├── PulseCheck.sln
│
└── .github/
    └── workflows/
        └── ci.yml
```

---

## How This Maps to Job Requirements

Based on actual 2025-2026 .NET job postings, here's what PulseCheck demonstrates:

| Job Requirement | Where It Shows Up in PulseCheck |
|----------------|-------------------------------|
| ASP.NET Core Web API | Entire backend |
| C# 12 / .NET 8 | Modern language features throughout |
| EF Core + SQL | Data layer, migrations, queries |
| REST API design | Endpoints follow REST conventions |
| Background processing | IHostedService for scheduled polling |
| Real-time (SignalR) | Live dashboard updates |
| React + TypeScript | Full frontend SPA |
| Docker | Containerized deployment |
| CI/CD (GitHub Actions) | Automated build/test pipeline |
| Azure deployment | Live hosted demo |
| Unit testing (xUnit) | Test project with meaningful coverage |
| Git/GitHub | Version control, PRs, open-source workflow |
| System design thinking | Architecture decisions documented in README |
