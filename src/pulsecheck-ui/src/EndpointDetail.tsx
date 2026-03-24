import { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'

function EndpointDetail() {
  const { id } = useParams() // gets the {id} from the URL
  const [history, setHistory] = useState([])

  useEffect(() => {
    fetch(`http://localhost:5063/api/endpoints/${id}/history`)
      .then(res => res.json())
      .then(data => setHistory(data))
  }, [id])

  return (
    <div>
      <Link to="/">← Back to Dashboard</Link>
      <h1>Endpoint History</h1>
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
