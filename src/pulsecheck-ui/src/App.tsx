// TODO: Next session — fetch latest HealthCheckResult per endpoint
// so the 🟢/🔴 dots show real isUp status, not just isActive
// Yogita, when you're back, tell Claude "let's fix the dots"
import './App.css'
import { useEffect, useState } from 'react'

function App() {
  // State to hold the list of endpoints from our API
  const [endpoints, setEndpoints] = useState([])

  // useEffect = "run this code when the component loads"
  // Like Page_Load in WebForms
  useEffect(() => {
   fetch('http://localhost:5063/api/endpoints/status')
      .then(res => res.json())
      .then(data => setEndpoints(data))
  }, [])

  return (
    <div>
      <h1>PulseCheck Dashboard</h1>
      {endpoints.map((ep: any) => (
        <div key={ep.id} className="endpoint-card">
        <span>{ep.latestCheck?.isUp ? '🟢' : '🔴'}</span>
        <span className="endpoint-name">{ep.name}</span>
        <span className="endpoint-url">{ep.url}</span>
        <span className="response-time">{ep.latestCheck ? `${ep.latestCheck.responseTimeMs}ms` : 'No data'}</span>
        <span className="response-time">{ep.latestCheck ? new Date(ep.latestCheck.checkedAt).toLocaleTimeString() : ''}</span>
      </div>

      ))}
    </div>
  )
}

export default App
