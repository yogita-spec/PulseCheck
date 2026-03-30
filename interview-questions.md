# PulseCheck - Interview Questions & Answers
## Complete Tech Stack Reference

---

# SECTION 1: C# & .NET BASICS

---

### Q1. What is .NET 8?
**A:** .NET 8 is the latest Long-Term Support (LTS) version of Microsoft's open-source, cross-platform framework for building applications. Unlike old .NET Framework (Windows-only), .NET 8 runs on Windows, Linux, and Mac.

**In PulseCheck:** Our entire backend API is built on .NET 8.

---

### Q2. What is the difference between .NET Framework and .NET 8?
**A:**
- .NET Framework is Windows only → .NET 8 is cross-platform (Windows, Linux, Mac)
- .NET Framework is closed source → .NET 8 is open source
- .NET Framework uses System.Web (heavy) → .NET 8 is lightweight, modular
- .NET Framework uses Global.asax, Startup.cs → .NET 8 uses Program.cs (minimal hosting)
- .NET Framework had slow releases → .NET 8 has yearly releases

---

### Q3. What is Program.cs? Why is there no Startup.cs?
**A:** In .NET 6+, Microsoft combined `Startup.cs` and `Program.cs` into a single `Program.cs` file using "minimal hosting." It's the entry point of the application where you:
- Register services (DI container)
- Configure middleware pipeline
- Map routes

**In PulseCheck:** Our Program.cs registers Controllers, DbContext, SignalR, CORS, and maps all endpoints.

---

### Q4. What is the difference between `var` and explicit type declaration?
**A:** `var` lets the compiler figure out the type automatically. It's not dynamic — the type is still fixed at compile time.
```csharp
var name = "Yogita";           // compiler knows it's string
string name = "Yogita";        // same thing, just explicit
var endpoints = db.MonitoredEndpoints.ToList();  // compiler knows it's List<MonitoredEndpoint>
```

---

### Q5. What is `async` and `await` in C#?
**A:** `async/await` allows your code to do other work while waiting for slow operations (database, network, file I/O) instead of blocking the thread.

- `async` marks a method as asynchronous
- `await` says "wait for this to finish, but don't block the thread"
- The return type is `Task` (no data back) or `Task<T>` (data back)

```csharp
// Without async — thread is BLOCKED, app freezes
var data = db.Endpoints.ToList();

// With async — thread is FREE to handle other requests
var data = await db.Endpoints.ToListAsync();
```

**In PulseCheck:** Every database call and HTTP call uses async/await.

---

### Q6. What is the difference between `Task`, `Task<T>`, and `void`?
**A:**
- `void` → Returns nothing, synchronous. Example: `void PrintHello()`
- `Task` → Returns nothing, but async (awaitable). Example: `async Task SaveAsync()`
- `Task<T>` → Returns data AND is async. Example: `async Task<List<Endpoint>> GetAllAsync()`

**Rule:** Never use `async void` except in event handlers — you can't await it and exceptions are lost.

---

# SECTION 2: ASP.NET CORE WEB API

---

### Q7. What is an API? What is a REST API?
**A:** An API (Application Programming Interface) is a way for two software systems to talk to each other. A REST API follows specific rules:
- Uses HTTP methods: GET (read), POST (create), PUT (update), DELETE (remove)
- Uses URLs to identify resources: `/api/endpoints`, `/api/endpoints/1`
- Returns data in JSON format
- Stateless — each request is independent

**Analogy:** Like a restaurant waiter — you (client) give an order (request), the waiter (API) goes to the kitchen (server), and brings back food (response).

---

### Q8. What are HTTP Status Codes?
**A:**
- **200** → OK, success (Sab theek hai)
- **201** → Created, new resource made (Naya record ban gaya)
- **204** → No Content, success but nothing to return (Kaam ho gaya, kuch dikhana nahi)
- **400** → Bad Request, client sent wrong data (Galat order diya)
- **404** → Not Found (Ghar pe koi nahi)
- **500** → Internal Server Error (Andar kuch toot gaya)

---

### Q9. What is a Controller in ASP.NET Core?
**A:** A Controller is a class that handles incoming HTTP requests and returns responses. It's like a waiter in a restaurant — it receives orders (requests) and sends back food (responses).

```csharp
[ApiController]
[Route("api/endpoints")]           // Base URL for all methods in this controller
public class EndpointsController : ControllerBase
{
    [HttpGet]                      // GET /api/endpoints
    public IActionResult GetAll() { ... }

    [HttpPost]                     // POST /api/endpoints
    public IActionResult Create() { ... }

    [HttpGet("{id}")]              // GET /api/endpoints/5
    public IActionResult GetById(int id) { ... }
}
```

**In PulseCheck:** `EndpointsController` handles all CRUD operations for monitored endpoints.

---

### Q10. What is the difference between `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`?
**A:** These are called **HTTP verb attributes** — they tell ASP.NET which HTTP method triggers which C# method:

- `[HttpGet]` → GET → Read/retrieve data. Example: Get all endpoints
- `[HttpPost]` → POST → Create new data. Example: Add a new endpoint
- `[HttpPut("{id}")]` → PUT → Update existing data. Example: Edit endpoint name/URL
- `[HttpDelete("{id}")]` → DELETE → Remove data. Example: Delete an endpoint

---

### Q11. What is `IActionResult`?
**A:** It's the return type for controller methods that lets you return different HTTP status codes:

```csharp
return Ok(data);           // 200 + data
return Created(...);       // 201
return NotFound();         // 404
return BadRequest();       // 400
```

Without `IActionResult`, you'd have to return a fixed type and couldn't control the status code.

---

### Q12. What is Model Binding in ASP.NET Core?
**A:** Model binding automatically converts incoming request data (JSON body, URL parameters, query strings) into C# objects.

```csharp
// URL parameter: /api/endpoints/5 → id = 5
[HttpGet("{id}")]
public IActionResult GetById(int id)

// JSON body → automatically becomes a MonitoredEndpoint object
[HttpPost]
public IActionResult Create(MonitoredEndpoint endpoint)
```

---

### Q13. What is Middleware in ASP.NET Core?
**A:** Middleware is code that runs on every request, in a pipeline (one after another), before reaching your controller. Like security checks at an airport — your request passes through each checkpoint in order.

```
Request → CORS → Authentication → Routing → Controller → Response
```

**In PulseCheck:** `app.UseCors()` is middleware that checks if the request is from an allowed origin.

---

### Q14. What is CORS? Why do we need it?
**A:** CORS (Cross-Origin Resource Sharing) is a browser security feature. When your React app (localhost:5173) calls your API (localhost:5063), the browser blocks it because they're on different ports (different "origins").

CORS tells the API: "Allow requests from this specific origin."

```csharp
policy.WithOrigins("http://localhost:5173")  // Allow React app
      .AllowAnyMethod()                      // Allow GET, POST, PUT, DELETE
      .AllowAnyHeader()                      // Allow Content-Type header
      .AllowCredentials()                    // Allow SignalR credentials
```

---

### Q15. What is Swagger/OpenAPI?
**A:** Swagger is an auto-generated UI that lets you test your API directly in the browser without writing any frontend code. It reads your controllers and creates a test page at `/swagger`.

**In PulseCheck:** We used Swagger to test our endpoints before building the React frontend.

---

# SECTION 3: DEPENDENCY INJECTION (DI)

---

### Q16. What is Dependency Injection?
**A:** Instead of a class creating its own dependencies (using `new`), the dependencies are "injected" from outside through the constructor.

**Analogy:** Instead of a class going to the market to buy its own ingredients, someone delivers them to the door. The class just says "I need an ILogger" and .NET hands it one.

```csharp
// WITHOUT DI — tightly coupled
public class MyController {
    private MyService service = new MyService();  // creates its own
}

// WITH DI — loosely coupled
public class MyController {
    private readonly MyService _service;
    public MyController(MyService service) {      // delivered by .NET
        _service = service;
    }
}
```

---

### Q17. What are the DI lifetimes in ASP.NET Core?
**A:**
- **Transient** (`AddTransient`) → New instance every time it's requested. Example: Lightweight services
- **Scoped** (`AddScoped`) → One instance per HTTP request. Example: DbContext
- **Singleton** (`AddSingleton`) → One instance for entire app lifetime. Example: Configuration, caching

**In PulseCheck:** `AddDbContext` registers as Scoped (one per request). `AddHostedService` registers as Singleton (one BackgroundHealthChecker for the whole app).

---

### Q18. What is `IServiceProvider` and `CreateScope()`?
**A:** `IServiceProvider` is like a master key — it can create any registered service. `CreateScope()` creates a temporary scope to get scoped services (like DbContext) from a singleton.

**In PulseCheck:** BackgroundHealthChecker is singleton but needs DbContext (scoped). It uses `CreateScope()` to get a fresh DbContext each time.

```csharp
using var scope = _serviceProvider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
```

---

# SECTION 4: ENTITY FRAMEWORK CORE

---

### Q19. What is Entity Framework Core?
**A:** EF Core is an ORM (Object-Relational Mapper) — it lets you work with databases using C# classes instead of writing SQL queries. Your C# classes become database tables.

**Analogy:** Instead of writing `SELECT * FROM Endpoints WHERE Id = 5`, you write `db.Endpoints.Find(5)` — EF Core translates it to SQL for you.

---

### Q20. What is a DbContext?
**A:** DbContext is the main class that talks to the database. It:
- Represents a session with the database
- Contains `DbSet<T>` properties (each one = a table)
- Tracks changes and saves them

```csharp
public class AppDbContext : DbContext
{
    public DbSet<MonitoredEndpoint> MonitoredEndpoints { get; set; }     // = Endpoints table
    public DbSet<HealthCheckResult> HealthCheckResults { get; set; }     // = Results table
}
```

**In PulseCheck:** `AppDbContext` manages our two tables.

---

### Q21. What are EF Core Migrations?
**A:** Migrations track changes to your C# models and apply them to the database. Like version control for your database schema.

```bash
dotnet ef migrations add AddEndpointTable    # Creates migration file
dotnet ef database update                     # Applies changes to DB
```

When you add a property to a model, you create a new migration — EF Core generates the SQL to alter the table.

---

### Q22. What is a Navigation Property and Foreign Key?
**A:** A foreign key links two tables. A navigation property lets you access related data through C# objects.

```csharp
public class HealthCheckResult
{
    public int MonitoredEndpointId { get; set; }              // Foreign key (the link)
    public MonitoredEndpoint Endpoint { get; set; } = null!;  // Navigation property (access related data)
}
```

**In PulseCheck:** Each HealthCheckResult belongs to one MonitoredEndpoint via the foreign key.

---

### Q23. What is LINQ? How is it used with EF Core?
**A:** LINQ (Language Integrated Query) lets you write database queries in C# syntax. EF Core translates LINQ to SQL.

```csharp
// LINQ query — EF Core converts this to SQL
var results = _context.HealthCheckResults
    .Where(h => h.MonitoredEndpointId == id)     // WHERE clause
    .OrderByDescending(h => h.CheckedAt)          // ORDER BY
    .Select(h => new { h.IsUp, h.ResponseTimeMs }) // SELECT columns
    .ToList();                                     // Execute query
```

---

### Q24. What is the difference between `ToList()` and `ToListAsync()`?
**A:** Both execute the query, but:
- `ToList()` — blocks the thread until the database responds
- `ToListAsync()` — frees the thread while waiting (use with `await`)

Always prefer `ToListAsync()` in web applications to handle more concurrent requests.

---

### Q25. What is the N+1 Query Problem?
**A:** When you load a list of items and then make a separate query for each item's related data:
- 1 query to get all endpoints
- N queries to get each endpoint's latest check

**Fix:** Use `.Include()` (eager loading) or `.Select()` with a subquery (projection) to get everything in fewer queries.

**In PulseCheck:** Our `GetStatus()` method uses `.Select()` with a subquery to avoid N+1.

---

# SECTION 5: BACKGROUND SERVICES

---

### Q26. What is a BackgroundService / IHostedService?
**A:** A BackgroundService runs tasks in the background automatically when the app starts — without any user request triggering it.

**Analogy:** Like a chowkidar (watchman) doing rounds — he checks every door every 30 seconds whether anyone asks or not.

```csharp
public class BackgroundHealthChecker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAllEndpointsAsync();
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
```

Registered with: `builder.Services.AddHostedService<BackgroundHealthChecker>();`

---

### Q27. What is a CancellationToken?
**A:** A signal that tells your code "the app is shutting down, stop what you're doing gracefully." Instead of killing the process abruptly, it gives your code a chance to clean up.

```csharp
while (!stoppingToken.IsCancellationRequested)  // "Keep going unless told to stop"
{
    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);  // "Wait 30s, but stop early if cancelled"
}
```

---

# SECTION 6: SIGNALR (REAL-TIME COMMUNICATION)

---

### Q28. What is SignalR?
**A:** SignalR is a library for adding real-time communication to your app. Instead of the client repeatedly asking the server for updates (polling), the server **pushes** updates to connected clients instantly.

**Analogy:** Without SignalR = sending letters (you ask, wait for reply). With SignalR = phone call (stay on the line, hear updates instantly).

**In PulseCheck:** When the chowkidar finishes pinging URLs, it broadcasts results to all connected browsers — the dashboard updates without refreshing.

---

### Q29. What is a SignalR Hub?
**A:** A Hub is a central class that manages real-time connections. Think of it as a conference call room — all connected browsers join the room, and the server can broadcast to everyone.

```csharp
public class HealthCheckHub : Hub
{
    public async Task SendHealthCheckUpdate(object results)
    {
        await Clients.All.SendAsync("ListenForPing", results);
    }
}
```

- `Clients.All` — send to every connected browser
- `"ListenForPing"` — the event name that JavaScript listens for

---

### Q30. What is `IHubContext<T>`? Why not inject the Hub directly?
**A:** `IHubContext<T>` is a "remote control" for the Hub. You can't inject the Hub directly into a singleton (like BackgroundHealthChecker) because the Hub is created per-connection. `IHubContext` lets you broadcast from anywhere.

```csharp
// In BackgroundHealthChecker — broadcasting without being a connected client
await _hubContext.Clients.All.SendAsync("ListenForPing", results);
```

---

### Q31. What is the difference between SignalR and regular API calls?
**A:**
- **Direction:** REST API = client asks server → SignalR = server pushes to client
- **Connection:** REST API = opens and closes each time → SignalR = stays open (persistent)
- **Use case:** REST API = CRUD operations → SignalR = real-time updates
- **Protocol:** REST API = HTTP → SignalR = WebSocket (upgrades from HTTP)

---

# SECTION 7: REACT & FRONTEND

---

### Q32. What is React?
**A:** React is a JavaScript library for building user interfaces. It breaks the UI into reusable **components** (like UserControls in WinForms) that update automatically when data changes.

---

### Q33. What is JSX?
**A:** JSX lets you write HTML-like syntax inside JavaScript. It looks like HTML but it's actually JavaScript that React converts.

```jsx
// This is JSX — looks like HTML but it's inside JavaScript
return <div>
    <h1>PulseCheck Dashboard</h1>
    <span>{endpoint.name}</span>    {/* curly braces = JavaScript expression */}
</div>
```

---

### Q34. What is `useState` in React?
**A:** `useState` creates a state variable — like a private field in a class, but when you change it, the UI **automatically re-renders**.

```typescript
const [endpoints, setEndpoints] = useState([])
//      ↑ value     ↑ setter function        ↑ initial value

setEndpoints(newData)   // Changes the value AND triggers UI re-render
```

**Analogy:** Like a private field, but with an automatic `Refresh()` call whenever you change it.

---

### Q35. What is `useEffect` in React?
**A:** `useEffect` runs side effects — code that should run when the component loads or when specific data changes. Like `Page_Load` in WebForms.

```typescript
// Runs ONCE when component loads (empty [] = no dependencies)
useEffect(() => {
    fetch('/api/endpoints/status')
        .then(res => res.json())
        .then(data => setEndpoints(data))
}, [])

// Runs every time `location` changes
useEffect(() => {
    fetchData()
}, [location])
```

The dependency array `[]` controls WHEN it re-runs.

---

### Q36. What are Props in React?
**A:** Props are like constructor parameters — the parent component passes data down to the child.

```jsx
// Parent passes data
<EndpointCard name="Google" url="https://google.com" />

// Child receives it
function EndpointCard({ name, url }) {
    return <div>{name} - {url}</div>
}
```

---

### Q37. What is React Router?
**A:** React Router enables navigation between different pages in a Single Page Application (SPA) without full page reloads.

```jsx
<Routes>
    <Route path="/" element={<Dashboard />} />
    <Route path="/endpoints/:id" element={<EndpointDetail />} />
</Routes>
```

- `useNavigate()` — programmatic navigation (like `Response.Redirect`)
- `useParams()` — read URL parameters (`:id`)
- `useLocation()` — get current URL info

**In PulseCheck:** Dashboard at `/`, endpoint detail at `/endpoints/:id`.

---

### Q38. What is a SPA (Single Page Application)?
**A:** A SPA loads one HTML page, then JavaScript swaps content in and out without full page reloads.

**Analogy:** Like a Gmail tab — the page never fully reloads, just the content inside changes.

Traditional: Click link → browser loads entirely new page from server
SPA: Click link → JavaScript updates just the part that changed

---

### Q39. What is the `fetch` API in JavaScript?
**A:** `fetch` is the built-in way to make HTTP requests from JavaScript (like `HttpClient` in C#).

```javascript
// GET request
fetch('http://localhost:5063/api/endpoints')
    .then(res => res.json())
    .then(data => console.log(data))

// POST request
fetch('http://localhost:5063/api/endpoints', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name: 'Google', url: 'https://google.com' })
})
```

---

# SECTION 8: TYPESCRIPT

---

### Q40. What is TypeScript? How is it different from JavaScript?
**A:** TypeScript is JavaScript with **types** — like how C# has types. It catches errors at compile time instead of runtime.

```typescript
// JavaScript — no type safety, error at runtime
let name = "Yogita"
name = 42              // no error until you run it

// TypeScript — type safety, error at compile time
let name: string = "Yogita"
name = 42              // ERROR: Type 'number' is not assignable to type 'string'
```

**In PulseCheck:** Our React app uses TypeScript (`.tsx` files) for type safety.

---

# SECTION 9: TOOLS & BUILD SYSTEM

---

### Q41. What is npm? What is package.json?
**A:** npm is the package manager for JavaScript — like NuGet for .NET.

- `npm install` → same as `dotnet restore` in .NET
- `package.json` → same as `.csproj` file in .NET
- `node_modules/` → same as `packages/` folder in .NET
- `npm run dev` → same as `dotnet run` in .NET

---

### Q42. What is Vite?
**A:** Vite is a fast build tool for React (and other frameworks). It:
- Runs a dev server with hot reload (changes appear instantly)
- Bundles your code for production
- Much faster than older tools like Webpack

**In PulseCheck:** `npm run dev` starts the Vite dev server at `localhost:5173`.

---

### Q43. What is NuGet?
**A:** NuGet is the package manager for .NET — lets you install third-party libraries.

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite    # Install EF Core SQLite
```

**In PulseCheck:** We installed EF Core, SQLite provider, and EF Core Tools via NuGet.

---

# SECTION 10: GIT & VERSION CONTROL

---

### Q44. What is Git? How is it different from TFS?
**A:** Git is a distributed version control system. Unlike TFS (centralized), every developer has a complete copy of the repository locally.

- Git is distributed → TFS is centralized
- `git commit` = local only → TFS check-in goes to server directly
- `git push` = send to server → TFS includes this in check-in
- `git pull` → same as TFS "Get Latest Version"
- Git branch = lightweight pointer → TFS branch = heavy copy

---

### Q45. What is the difference between `git add`, `git commit`, and `git push`?
**A:**
```
Working Directory → git add → Staging Area → git commit → Local Repo → git push → Remote (GitHub)
```

- `git add` — "I want to include these files" (like selecting files for check-in)
- `git commit` — "Save a snapshot locally with a message" (local only!)
- `git push` — "Send my commits to GitHub" (like TFS check-in)

---

### Q46. What is .gitignore?
**A:** A file that tells Git which files/folders to NOT track. Used for:
- `node_modules/` — too large, can be recreated with `npm install`
- `bin/`, `obj/` — build output, recreated on build
- `.env` — secrets that shouldn't be in source control
- `*.db` — local database files

---

### Q47. What is a Pull Request (PR)?
**A:** A request to merge your branch into the main branch, with code review. Other developers can review, comment, and approve before the code is merged.

Like TFS Pull Request — code review before merge.

---

# SECTION 11: DATABASE

---

### Q48. What is SQLite?
**A:** SQLite is a lightweight, file-based database. No server installation needed — the entire database is a single `.db` file.

- SQLite is file-based (`.db` file) → SQL Server is server-based
- SQLite needs no installation → SQL Server requires server setup
- SQLite is great for development/small apps → SQL Server for production workloads
- SQLite is single user → SQL Server supports multi-user concurrent access

**In PulseCheck:** We use SQLite for local development. Easy to switch to SQL Server later by changing the connection string.

---

### Q49. What is a Connection String?
**A:** A string that tells your app how to connect to the database — which server, which database, credentials, etc.

```json
// appsettings.json
"ConnectionStrings": {
    "DefaultConnection": "Data Source=pulsecheck.db"    // SQLite — just a file path
}
```

For SQL Server it would include server name, database name, and authentication.

---

# SECTION 12: GENERAL CONCEPTS

---

### Q50. What is JSON?
**A:** JSON (JavaScript Object Notation) is a lightweight data format for exchanging data between systems. Like XML but simpler.

```json
{
    "id": 1,
    "name": "Google",
    "url": "https://google.com",
    "isActive": true
}
```

Your API sends JSON responses, and React reads them with `.json()`.

---

### Q51. What is HttpClient in .NET?
**A:** A class for making HTTP requests from C# — like `fetch` in JavaScript.

```csharp
var response = await _httpClient.GetAsync(url);
var statusCode = (int)response.StatusCode;
```

**In PulseCheck:** `HealthCheckService` uses HttpClient to ping URLs and check if they're up.

---

### Q52. What is the Stopwatch class?
**A:** A class for measuring elapsed time with high precision.

```csharp
var sw = Stopwatch.StartNew();
await _httpClient.GetAsync(url);
sw.Stop();
var responseTimeMs = (int)sw.ElapsedMilliseconds;   // How long the request took
```

**In PulseCheck:** Used to measure response time of each health check.

---

### Q53. What is the difference between Scoped, Transient, and Singleton?
**A:** (Most common interview question about DI!)

- **Transient** → Created every time requested, destroyed immediately after use. Use for: lightweight, stateless services
- **Scoped** → Created once per HTTP request, destroyed at end of request. Use for: DbContext, per-request data
- **Singleton** → Created once when app starts, destroyed when app stops. Use for: Configuration, BackgroundService, caching

**Trick question:** What happens if a Singleton depends on a Scoped service?
**Answer:** It causes a "captive dependency" bug — the scoped service lives forever inside the singleton. Use `IServiceProvider.CreateScope()` to fix it (like we did in BackgroundHealthChecker).

---

### Q54. What is the difference between `==` and `===` in JavaScript?
**A:**
```javascript
5 == "5"     // true  — compares value only (converts types)
5 === "5"    // false — compares value AND type (strict)
```

Always use `===` to avoid unexpected type conversion bugs.

---

### Q55. What is `?.` (optional chaining) in JavaScript/TypeScript?
**A:** Safely access nested properties without crashing if something is null/undefined.

```typescript
ep.latestCheck?.isUp      // If latestCheck is null, returns undefined instead of crashing
// Same as:
ep.latestCheck ? ep.latestCheck.isUp : undefined
```

**In PulseCheck:** Used in the dashboard to safely show the green/red dot.

---

### Q56. What is a `.map()` function in JavaScript?
**A:** Transforms each item in an array into something new. Like LINQ's `.Select()` in C#.

```javascript
// JavaScript
endpoints.map(ep => <div>{ep.name}</div>)

// Equivalent C# LINQ
endpoints.Select(ep => new { ep.Name })
```

**In PulseCheck:** Used to render the list of endpoint cards on the dashboard.

---

### Q57. What is template literal in JavaScript?
**A:** A string with backticks that allows embedding expressions using `${}`.

```javascript
`${ep.latestCheck.responseTimeMs}ms`     // "120ms"
`/endpoints/${ep.id}`                     // "/endpoints/5"
```

Like string interpolation in C#: `$"{responseTime}ms"`

---

### Q58. What is Recharts?
**A:** A React charting library for building charts (line, bar, pie, etc.) using React components.

**In PulseCheck:** We use `LineChart` from Recharts to show response time trends on the endpoint detail page.

---

### Q59. What is the difference between controlled and uncontrolled components in React?
**A:**
- **Controlled:** React state drives the input value. You use `useState` + `onChange`.
- **Uncontrolled:** The DOM handles the value. You use `useRef` to read it.

```jsx
// Controlled (what we use in PulseCheck)
<input value={newName} onChange={e => setNewName(e.target.value)} />
```

Our Add Endpoint form uses controlled components — React controls every keystroke.

---

### Q60. Explain the complete flow of PulseCheck — how does a URL get monitored?
**A:** (This is the "walk me through the architecture" question)

1. **User adds a URL** via the React dashboard form
2. React sends a `POST /api/endpoints` request to the API
3. `EndpointsController.Create()` saves it to the SQLite database via EF Core
4. `BackgroundHealthChecker` (running every 30 seconds) picks it up from the database
5. It calls `HealthCheckService.CheckAsync()` which uses `HttpClient` to ping the URL
6. The result (isUp, statusCode, responseTimeMs) is saved as a `HealthCheckResult` in the database
7. BackgroundHealthChecker broadcasts via `IHubContext` → SignalR Hub → `"ListenForPing"` event
8. React's SignalR connection hears the event and re-fetches the dashboard data
9. The dashboard updates in real-time showing green/red dots, response times, and timestamps

**Tech stack in one request flow:** React (TypeScript) → HTTP/CORS → ASP.NET Core Controller → EF Core → SQLite → BackgroundService → HttpClient → SignalR Hub → WebSocket → React re-render

---

## Tips for the Interview

1. **Always relate to PulseCheck** — "In my project, I used this to..."
2. **Know the WHY, not just the WHAT** — "We chose SignalR because polling wastes resources"
3. **Be honest about learning** — "I started with .NET Framework and transitioned to .NET 8"
4. **Draw diagrams** if they ask about architecture — the request flow diagram above is gold
5. **Practice Q60** — the end-to-end flow question comes up in almost every full-stack interview

---

---

# SECTION 13: DOCKER & CONTAINERS

---

### Q61. What is Docker and how is it different from hosting on IIS?
**A:**
- **IIS** is a web server — it receives HTTP requests and runs your app. But you have to manually install .NET, configure application pools, set up bindings, and make sure the server has the right runtime version.
- **Docker** is a packaging system — it packs your app + runtime + everything it needs into one container. That container runs the same on any machine (your laptop, Azure, AWS, Linux server).
- Docker containers use **Kestrel** (ASP.NET Core's built-in web server) instead of IIS, so you don't need IIS at all.

**Analogy:** IIS is like a restaurant kitchen — it can cook, but you have to set it up. Docker is like a tiffin box — the food is already packed, just open it anywhere.

**In PulseCheck:** We containerized our .NET 8 API using a Dockerfile so it can be deployed anywhere without worrying about server setup.

| | **IIS** | **Docker** |
|---|---|---|
| What it is | Web server | Packaging system |
| Setup needed | Install .NET, configure app pool, bindings | Just run the container |
| Portability | Windows only (IIS is Windows) | Runs anywhere (Windows, Linux, cloud) |
| Dependencies | Must match server's .NET version | Carries its own .NET version inside |

---

*Generated from the PulseCheck project — built as a learning project covering .NET 8, EF Core, React, TypeScript, SignalR, SQLite, and Docker.*
