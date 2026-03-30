import { useEffect, useState } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'
import {
  LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer
} from 'recharts'

function EndpointDetail() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [history, setHistory] = useState([])

const [endpoint, setEndpoint] = useState<any>(null)
const [editMode, setEditMode] = useState(false)
const [editName, setEditName] = useState('')
const [editUrl, setEditUrl] = useState('')



  const handleDelete = async () => {
    const res = await fetch(`http://localhost:5063/api/endpoints/${id}`, 
      { method: 'DELETE' })
     if (res.ok) navigate('/')
  }

  const handleEdit = async () => {
    const res = await fetch(`http://localhost:5063/api/endpoints/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name: editName, url: editUrl, isActive: true })
    })
    if (res.ok) setEditMode(false)
  }




  useEffect(() => {
    fetch(`http://localhost:5063/api/endpoints/${id}/history`)
      .then(res => res.json())
      .then(data => setHistory(data))

    fetch(`http://localhost:5063/api/endpoints/${id}`)
      .then(res => res.json())
      .then(data => {
        setEndpoint(data)
        setEditName(data.name)
        setEditUrl(data.url)
  })
  }, [id])

  // Transform history data for the chart
  // Each point needs a label for X axis and a value for Y axis
  const chartData = history.map((h: any) => ({
    time: new Date(h.checkedAt).toLocaleTimeString(),
    responseMs: h.responseTimeMs
  }))

  return (
    <div>
      <Link to="/">← Back to Dashboard</Link>
      <h1>Endpoint History</h1>
      <button onClick={handleDelete} style={{ color: 'red' }}>Delete Endpoint</button>
      <button onClick={() => setEditMode(!editMode)}>Edit Endpoint</button>

      {editMode && (
        <div>
          <input value={editName} onChange={e => setEditName(e.target.value)} />
          <input value={editUrl} onChange={e => setEditUrl(e.target.value)} />
          <button onClick={handleEdit}>Save</button>
          <button onClick={() => setEditMode(false)}>Cancel</button>
        </div>
      )}


      <h2>Response Time Chart</h2>
      {
        <ResponsiveContainer width="100%" height={300}>
        <LineChart data={chartData}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="time" />
          <YAxis />
          <Tooltip />
          <Line type="monotone" dataKey="responseMs" stroke="#4caf50" />
        </LineChart>
      </ResponsiveContainer>
}

      {history.map((h: any) => (
        <div key={h.id} className="endpoint-card">
          <span>{h.isUp ? '🟢' : '🔴'}</span>
          <span className="response-time">{h.responseTimeMs}ms</span>
          <span className="response-time">{new Date(h.checkedAt).toLocaleString()}</span>
          <span className="response-time">Status: {h.statusCode}</span>
        </div>
      ))}
    </div>
  )
}

export default EndpointDetail
