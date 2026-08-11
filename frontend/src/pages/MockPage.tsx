import { useEffect, useMemo, useState } from 'react'
import { api } from '../api'
import type { MockSession } from '../types'
import { MarkdownView } from '../components/MarkdownView'

export function MockPage() {
  const [mode, setMode] = useState('hld')
  const [minutes, setMinutes] = useState(30)
  const [session, setSession] = useState<MockSession | null>(null)
  const [input, setInput] = useState('')
  const [busy, setBusy] = useState(false)
  const [now, setNow] = useState(Date.now())

  useEffect(() => {
    if (!session || session.endedAt) return
    const t = setInterval(() => setNow(Date.now()), 1000)
    return () => clearInterval(t)
  }, [session])

  const remaining = useMemo(() => {
    if (!session) return null
    const end = new Date(session.startedAt).getTime() + session.durationMinutes * 60_000
    return Math.max(0, Math.floor((end - now) / 1000))
  }, [session, now])

  async function start() {
    setBusy(true)
    try {
      setSession(await api.startMock(mode, minutes))
    } finally {
      setBusy(false)
    }
  }

  async function send() {
    if (!session || !input.trim()) return
    setBusy(true)
    try {
      setSession(await api.mockMessage(session.id, input.trim()))
      setInput('')
    } finally {
      setBusy(false)
    }
  }

  async function end() {
    if (!session) return
    setBusy(true)
    try {
      setSession(await api.endMock(session.id))
    } finally {
      setBusy(false)
    }
  }

  return (
    <section className="mock">
      <h1>Timed mock</h1>
      <p className="lede">HLD, CS fundamentals, or behavioral — one question at a time.</p>

      {!session && (
        <div className="mock-setup">
          <label>
            Mode
            <select value={mode} onChange={(e) => setMode(e.target.value)}>
              <option value="hld">HLD</option>
              <option value="cs">CS fundamentals</option>
              <option value="behavioral">Behavioral</option>
            </select>
          </label>
          <label>
            Minutes
            <input
              type="number"
              min={5}
              max={90}
              value={minutes}
              onChange={(e) => setMinutes(Number(e.target.value))}
            />
          </label>
          <button type="button" className="btn" disabled={busy} onClick={() => void start()}>
            Start
          </button>
        </div>
      )}

      {session && (
        <>
          <div className="mock-bar">
            <span className="tag">{session.mode}</span>
            <span>
              {session.endedAt
                ? 'Ended'
                : remaining != null
                  ? `${Math.floor(remaining / 60)}:${String(remaining % 60).padStart(2, '0')} left`
                  : ''}
            </span>
            {!session.endedAt && (
              <button type="button" className="btn btn--ghost" onClick={() => void end()}>
                End & rubric
              </button>
            )}
          </div>
          <div className="chat-log chat-log--wide">
            {session.transcript.map((m) => (
              <div key={m.id} className={`bubble bubble--${m.role}`}>
                <MarkdownView content={m.content} />
              </div>
            ))}
          </div>
          {session.rubric && (
            <div className="draft-card">
              <h2>Rubric</h2>
              <MarkdownView content={session.rubric} />
            </div>
          )}
          {!session.endedAt && (
            <div className="chat-input">
              <textarea rows={3} value={input} onChange={(e) => setInput(e.target.value)} placeholder="Your answer…" />
              <button type="button" className="btn" disabled={busy} onClick={() => void send()}>
                Reply
              </button>
            </div>
          )}
        </>
      )}
    </section>
  )
}
