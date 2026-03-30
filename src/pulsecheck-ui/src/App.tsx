// TODO: Next session — TWO things to do (in this order):
//
// 1. FIRST: Clean up the GitHub repo — CLAUDE.md, flashcards.md, progress.md,
//    session-journal.md, NEXT-SESSION.md, dev-notes.md, PulseCheck-Project-Scope.md
//    are all showing publicly on GitHub. They need to be removed from Git tracking
//    (git rm --cached) AND added to .gitignore, then pushed.
//    Tell Claude: "let's clean up the repo"
//
// 2. THEN: SignalR real-time updates — the dashboard will update live, no refresh needed.
//    Tell Claude: "let's make the dashboard live"
import './App.css'
import { useEffect, useState } from 'react'
import { Routes, Route, useNavigate,useLocation } from 'react-router-dom'
import EndpointDetail from './EndpointDetail'
import * as signalR from '@microsoft/signalr'



function App() {
  // State to hold the list of endpoints from our API
const [endpoints, setEndpoints] = useState([])
  // State for the form inputs
const [newName, setNewName] = useState('')
const [newUrl, setNewUrl] = useState('')
const navigate = useNavigate()
const location = useLocation()

useEffect(() => {
  // Build the connection — like dialing the conference room
  const connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5063/hub/healthcheck')
    .withAutomaticReconnect()
    .build()

  // Listen for the chowkidar's broadcast
  connection.on('ListenForPing', () => {
    fetch('http://localhost:5063/api/endpoints/status')
      .then(res => res.json())
      .then(data => setEndpoints(data))
  })

  // Start the connection
  connection.start()
    .then(() => console.log('SignalR connected!'))
    .catch(err => console.error('SignalR failed:', err))

  // Cleanup — hang up when page closes
  return () => { connection.stop() }
}, [])



// Called when form is submitted
  const handleSubmit = (e: any) => {
    e.preventDefault() // stops page from refreshing — like e.Handled = true

    fetch('http://localhost:5063/api/endpoints', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name: newName, url: newUrl, isActive: true })
    })
    .then(res => res.json())
    .then(() => {
      // Refresh the list after adding
      fetch('http://localhost:5063/api/endpoints/status')
        .then(res => res.json())
        .then(data => setEndpoints(data))
      // Clear the form
      setNewName('')
      setNewUrl('')
    })
  }



  // useEffect = "run this code when the component loads"
  // Like Page_Load in WebForms
  useEffect(() => {
   fetch('http://localhost:5063/api/endpoints/status')
      .then(res => res.json())
      .then(data => setEndpoints(data))
  }, [location])

  return (
  <Routes>
    <Route path="/" element={
      <div>
        <h1>PulseCheck Dashboard</h1>
        <form onSubmit={handleSubmit} style={{ marginBottom: '20px' }}>
          <input type="text" placeholder="Name" value={newName} onChange={e => setNewName(e.target.value)} />
          <input type="text" placeholder="URL (e.g. https://example.com)" value={newUrl} onChange={e => setNewUrl(e.target.value)} />
          <button type="submit">Add Endpoint</button>
        </form>
        {endpoints.map((ep: any) => (
          <div key={ep.id} className="endpoint-card" onClick={() => navigate(`/endpoints/${ep.id}`)} style={{ cursor: 'pointer' }}>
            <span>{ep.latestCheck?.isUp ? '🟢' : '🔴'}</span>
            <span className="endpoint-name">{ep.name}</span>
            <span className="endpoint-url">{ep.url}</span>
            <span className="response-time">{ep.latestCheck ? `${ep.latestCheck.responseTimeMs}ms` : 'No data'}</span>
            <span className="response-time">{ep.latestCheck ? new Date(ep.latestCheck.checkedAt).toLocaleTimeString() : ''}</span>
          </div>
        ))}
      </div>
    } />
    <Route path="/endpoints/:id" element={<EndpointDetail />} />
  </Routes>
)

}

export default App
