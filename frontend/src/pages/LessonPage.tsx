import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { api } from '../api'
import type { Lesson } from '../types'
import { ChatPanel } from '../components/ChatPanel'
import { MarkdownView } from '../components/MarkdownView'

export function LessonPage() {
  const { slug = '', id = '' } = useParams()
  const [lesson, setLesson] = useState<Lesson | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api
      .lesson(slug, id)
      .then(setLesson)
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
  }, [slug, id])

  async function toggleComplete() {
    if (!lesson) return
    const next = !lesson.completed
    await api.complete(lesson.id, next)
    setLesson({ ...lesson, completed: next })
  }

  if (error) return <p className="error">{error}</p>
  if (!lesson) return <p className="muted">Loading lesson…</p>

  return (
    <div className="lesson-layout">
      <article className="lesson">
        <p className="crumb">
          <Link to="/tracks">Tracks</Link> / {lesson.trackSlug} / {lesson.module}
        </p>
        <header className="lesson-head">
          <div>
            <h1>{lesson.title}</h1>
            <p className="lede">{lesson.summary}</p>
            <div className="meta">
              <span className="tag">{lesson.source}</span>
              {lesson.languages.map((l) => (
                <span key={l} className="tag">
                  {l}
                </span>
              ))}
            </div>
          </div>
          <button type="button" className="btn" onClick={() => void toggleComplete()}>
            {lesson.completed ? 'Mark incomplete' : 'Mark complete'}
          </button>
        </header>
        <MarkdownView content={lesson.markdownBody ?? ''} />
      </article>
      <ChatPanel lessonId={lesson.id} trackSlug={lesson.trackSlug} />
    </div>
  )
}
