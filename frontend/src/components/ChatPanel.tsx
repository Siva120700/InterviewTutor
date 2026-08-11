import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api } from '../api'
import type { ChatMessage, Lesson, LessonDraft } from '../types'
import { MarkdownView } from './MarkdownView'

const MODES = [
  { id: '', label: 'Ask' },
  { id: 'explain_simple', label: 'Simple' },
  { id: 'explain_deep', label: 'Deep' },
  { id: 'interview', label: 'Interview' },
  { id: 'walkthrough', label: 'Walk code' },
  { id: 'example', label: 'Example' },
  { id: 'quiz', label: 'Quiz me' },
]

export function ChatPanel({ lessonId }: { lessonId: string; trackSlug: string }) {
  const [messages, setMessages] = useState<ChatMessage[]>([])
  const [threadId, setThreadId] = useState<string | null>(null)
  const [input, setInput] = useState('')
  const [mode, setMode] = useState('')
  const [lang, setLang] = useState('java')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [draft, setDraft] = useState<LessonDraft | null>(null)
  const [suggested, setSuggested] = useState<Lesson | null>(null)

  useEffect(() => {
    let alive = true
    api
      .getChat(lessonId)
      .then((t) => {
        if (!alive || !t) return
        setThreadId(t.id)
        setMessages(t.messages ?? [])
      })
      .catch(() => {})
    return () => {
      alive = false
    }
  }, [lessonId])

  async function send() {
    if (!input.trim() || busy) return
    setBusy(true)
    setError(null)
    const text = input.trim()
    setInput('')
    setMessages((m) => [
      ...m,
      { id: crypto.randomUUID(), role: 'user', content: text, createdAt: new Date().toISOString() },
    ])
    try {
      const res = await api.sendChat(lessonId, {
        message: text,
        mode: mode || null,
        preferredLanguage: lang,
        threadId,
      })
      setThreadId(res.thread.id)
      setMessages(res.thread.messages)
      setDraft(res.draft ?? null)
      setSuggested(res.suggestedExisting ?? null)
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Chat failed')
    } finally {
      setBusy(false)
    }
  }

  async function confirmDraft() {
    if (!draft) return
    setBusy(true)
    try {
      const lesson = await api.confirmDraft(draft.id)
      setDraft(null)
      window.location.href = `/tracks/${lesson.trackSlug}/lessons/${lesson.id}`
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Confirm failed')
    } finally {
      setBusy(false)
    }
  }

  async function cancelDraft() {
    if (!draft) return
    await api.cancelDraft(draft.id)
    setDraft(null)
  }

  return (
    <aside className="chat-panel">
      <header className="chat-panel__head">
        <h2>Doubt chat</h2>
        <select value={lang} onChange={(e) => setLang(e.target.value)} aria-label="Language">
          <option value="java">Java</option>
          <option value="csharp">C#</option>
        </select>
      </header>

      <div className="mode-row">
        {MODES.map((m) => (
          <button
            key={m.id || 'ask'}
            type="button"
            className={mode === m.id ? 'chip chip--on' : 'chip'}
            onClick={() => setMode(m.id)}
          >
            {m.label}
          </button>
        ))}
      </div>

      <div className="chat-log">
        {messages.length === 0 && (
          <p className="muted">Ask anything about this lesson. Try “add a lesson on Redis...” to draft a new topic.</p>
        )}
        {messages.map((m) => (
          <div key={m.id} className={`bubble bubble--${m.role}`}>
            <MarkdownView content={m.content} />
          </div>
        ))}
      </div>

      {suggested && (
        <div className="draft-card">
          <strong>Similar lesson exists</strong>
          <p>
            {suggested.title} — open{' '}
            <Link to={`/tracks/${suggested.trackSlug}/lessons/${suggested.id}`}>{suggested.id}</Link>
          </p>
        </div>
      )}

      {draft && (
        <div className="draft-card">
          <strong>Draft: {draft.title}</strong>
          <p className="muted">
            Track: {draft.trackSlug} · {draft.summary}
          </p>
          <details>
            <summary>Preview markdown</summary>
            <MarkdownView content={draft.markdownBody} />
          </details>
          <div className="row">
            <button type="button" className="btn" onClick={confirmDraft} disabled={busy}>
              Add to my lessons
            </button>
            <button type="button" className="btn btn--ghost" onClick={cancelDraft}>
              Cancel
            </button>
          </div>
        </div>
      )}

      {error && <p className="error">{error}</p>}

      <div className="chat-input">
        <textarea
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Ask a doubt…"
          rows={3}
          onKeyDown={(e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
              e.preventDefault()
              void send()
            }
          }}
        />
        <button type="button" className="btn" disabled={busy} onClick={() => void send()}>
          {busy ? '…' : 'Send'}
        </button>
      </div>
    </aside>
  )
}
