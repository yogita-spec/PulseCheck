# PulseCheck - API Health Monitor

A real-time API health monitoring dashboard built with .NET 8 and React. PulseCheck continuously pings your API endpoints, tracks response times, and shows live status updates via SignalR.

![PulseCheck Dashboard](screenshots/dashboard.png)
![URL History](screenshots/endpoint-history.png)


## Features

- Add, edit, and delete API endpoints to monitor
- Automatic health checks every 30 seconds (background service)
- Real-time dashboard updates via SignalR (no page refresh needed)
- Response time tracking with interactive charts (Recharts)
- Health check history for each endpoint
- Green/red status indicators for up/down endpoints
- RESTful API with full CRUD operations
- Dockerized with docker-compose for easy setup
- CI/CD pipeline with GitHub Actions
- Unit tests with xUnit (8 tests)



## Tech Stack

### Backend
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8 (SQLite)
- SignalR for real-time communication
- Background service for automated health checks

### Frontend
- React 18 with TypeScript
- Vite (build tool)
- Recharts (response time charts)

### DevOps
- Docker (multi-stage build)
- docker-compose
- GitHub Actions CI/CD pipeline
- xUnit for unit testing


## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- Docker Desktop (optional, for containerized setup)

### Run with Docker (easiest)

```bash
docker-compose up
```

### Run without Docker

```bash
# Start the API
dotnet run --project src/PulseCheck.Api

# In a separate terminal, start the React frontend
cd src/pulsecheck-ui
npm install
npm run dev
```

### Run Tests

```bash
dotnet test
```

## Project Structure

```
PulseCheck/
├── src/
│   ├── PulseCheck.Api/          # .NET 8 Web API
│   │   ├── Controllers/         # API endpoints
│   │   ├── Services/            # Health check logic
│   │   ├── Models/              # Domain models
│   │   ├── Data/                # EF Core DbContext
│   │   ├── Hubs/                # SignalR hub
│   │   └── BackgroundServices/  # Automated health checker
│   └── pulsecheck-ui/           # React frontend
│       └── src/
│           ├── components/      # React components
│           └── services/        # API client
├── tests/                       # xUnit tests
├── Dockerfile                   # Multi-stage Docker build
├── docker-compose.yml           # Full stack setup
└── .github/workflows/ci.yml    # GitHub Actions CI
```