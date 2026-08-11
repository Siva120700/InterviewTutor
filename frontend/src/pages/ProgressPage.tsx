import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api } from '../api'
import type { Lesson, Progress } from '../types'

export function ProgressPage() {
  const [progress, setProgress] = useState<Progress | null>(null)
  const [suggested, setSuggested] = useState<Lesson[]>([])
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    Promise.all([api.progress(), api.suggested()])
      .then(([p, s]) => {
        setProgress(p)
        setSuggested(s)
      })
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
  }, [])

  if (error) return <p className="error">{error}</p>
  if (!progress) return <p className="muted">Loading…</p>

  const pct = progress.totalLessons === 0 ? 0 : Math.round((progress.completedCount / progress.totalLessons) * 100)

  return (
    <section>
      <h1>Progress</h1>
      <p className="lede">
        {progress.completedCount} of {progress.totalLessons} lessons complete ({pct}%).
      </p>
      <div className="bar" aria-hidden>
        <div className="bar__fill" style={{ width: `${pct}%` }} />
      </div>
      <h2>Suggested next</h2>
      <ul className="lesson-list">
        {suggested.map((l) => (
          <li key={l.id}>
            <Link to={`/tracks/${l.trackSlug}/lessons/${l.id}`}>
              {l.title} <span className="muted">({l.trackSlug})</span>
            </Link>
          </li>
        ))}
      </ul>
    </section>
  )
}
