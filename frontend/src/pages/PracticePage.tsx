import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { api } from '../api'
import type { Problem } from '../types'
import { MarkdownView } from '../components/MarkdownView'

export function PracticePage() {
  const { slug } = useParams()
  const [problems, setProblems] = useState<Problem[]>([])
  const [selected, setSelected] = useState<Problem | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api
      .problems()
      .then(setProblems)
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
  }, [])

  useEffect(() => {
    if (!slug) {
      setSelected(null)
      return
    }
    api
      .problem(slug)
      .then(setSelected)
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
  }, [slug])

  if (error) return <p className="error">{error}</p>

  if (selected) {
    return (
      <section>
        <p className="crumb">
          <Link to="/practice">Practice</Link> / {selected.slug}
        </p>
        <h1>{selected.title}</h1>
        <p className="meta">
          <span className="tag">{selected.difficulty}</span>
          <span className="tag">{selected.trackSlug}</span>
        </p>
        <MarkdownView content={selected.promptMarkdown} />
        <h2>Java</h2>
        <MarkdownView content={`\`\`\`java\n${selected.javaSolution ?? ''}\n\`\`\``} />
        <h2>C#</h2>
        <MarkdownView content={`\`\`\`csharp\n${selected.csharpSolution ?? ''}\n\`\`\``} />
        <h2>Complexity</h2>
        <p>{selected.complexityNotes}</p>
      </section>
    )
  }

  return (
    <section>
      <h1>Practice</h1>
      <p className="lede">Small dual-language problem bank — no online judge.</p>
      <ul className="problem-list">
        {problems.map((p) => (
          <li key={p.slug}>
            <Link to={`/practice/${p.slug}`}>
              <strong>{p.title}</strong>
              <span className="muted">
                {p.difficulty} · {p.trackSlug}
              </span>
            </Link>
          </li>
        ))}
      </ul>
    </section>
  )
}
