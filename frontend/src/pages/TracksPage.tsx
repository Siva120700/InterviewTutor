import { useEffect, useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { api } from '../api'
import type { Track } from '../types'

export function TracksPage() {
  const [tracks, setTracks] = useState<Track[]>([])
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api
      .tracks()
      .then(setTracks)
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
  }, [])

  const groups = useMemo(() => {
    const map = new Map<string, Track[]>()
    for (const t of tracks) {
      const list = map.get(t.group) ?? []
      list.push(t)
      map.set(t.group, list)
    }
    return [...map.entries()]
  }, [tracks])

  if (error) return <p className="error">{error}</p>

  return (
    <section>
      <h1>Tracks</h1>
      <p className="lede">Grouped catalog of interview, CS, and language lessons.</p>
      {groups.map(([group, list]) => (
        <div key={group} className="group">
          <h2>{group}</h2>
          <div className="track-grid">
            {list.map((t) => {
              const count = t.modules.reduce((n, m) => n + m.lessons.length, 0)
              const done = t.modules.reduce((n, m) => n + m.lessons.filter((l) => l.completed).length, 0)
              return (
                <article key={t.slug} className="track-block">
                  <header>
                    <h3>{t.title}</h3>
                    <span className="muted">
                      {done}/{count}
                    </span>
                  </header>
                  <p>{t.description}</p>
                  <ul className="lesson-list">
                    {t.modules.map((m) => (
                      <li key={m.name}>
                        <strong>{m.name}</strong>
                        <ul>
                          {m.lessons.map((l) => (
                            <li key={l.id}>
                              <Link to={`/tracks/${t.slug}/lessons/${l.id}`}>
                                {l.completed ? '✓ ' : ''}
                                {l.title}
                              </Link>
                              {l.source === 'user_requested' && <span className="tag">yours</span>}
                            </li>
                          ))}
                        </ul>
                      </li>
                    ))}
                  </ul>
                </article>
              )
            })}
          </div>
        </div>
      ))}
    </section>
  )
}
