import { useEffect, useMemo, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { api } from '../api'
import type { DsaSheet, Problem, SheetProblem } from '../types'
import { MarkdownView } from '../components/MarkdownView'

function problemHref(p: SheetProblem): string | null {
  if (p.problemLink && p.problemLink !== 'NA') return p.problemLink
  if (p.articleLink) return p.articleLink
  return null
}

export function PracticePage() {
  const { slug } = useParams()
  const [tab, setTab] = useState<'sheet' | 'worked'>('sheet')
  const [problems, setProblems] = useState<Problem[]>([])
  const [sheet, setSheet] = useState<DsaSheet | null>(null)
  const [selected, setSelected] = useState<Problem | null>(null)
  const [query, setQuery] = useState('')
  const [openGroup, setOpenGroup] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api
      .problems()
      .then(setProblems)
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
    api
      .dsaSheet()
      .then((s) => {
        setSheet(s)
        setOpenGroup(s.groups[0]?.id ?? null)
      })
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load sheet'))
  }, [])

  useEffect(() => {
    if (!slug) {
      setSelected(null)
      return
    }
    setTab('worked')
    api
      .problem(slug)
      .then(setSelected)
      .catch((e) => setError(e instanceof Error ? e.message : 'Failed to load'))
  }, [slug])

  const filteredGroups = useMemo(() => {
    if (!sheet) return []
    const q = query.trim().toLowerCase()
    if (!q) return sheet.groups
    return sheet.groups
      .map((g) => ({
        ...g,
        subgroups: g.subgroups
          .map((sg) => ({
            ...sg,
            problems: sg.problems.filter(
              (p) =>
                p.title?.toLowerCase().includes(q) ||
                p.difficulty?.toLowerCase().includes(q) ||
                sg.title.toLowerCase().includes(q) ||
                g.title.toLowerCase().includes(q),
            ),
          }))
          .filter((sg) => sg.problems.length > 0),
      }))
      .filter((g) => g.subgroups.length > 0)
  }, [sheet, query])

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
      <p className="lede">
        Full topic-wise DSA sheet (LearnYard-structured) plus a small dual-language worked set.
      </p>

      <div className="tabs">
        <button type="button" className={tab === 'sheet' ? 'tab active' : 'tab'} onClick={() => setTab('sheet')}>
          DSA Sheet
        </button>
        <button type="button" className={tab === 'worked' ? 'tab active' : 'tab'} onClick={() => setTab('worked')}>
          Worked solutions
        </button>
      </div>

      {tab === 'worked' && (
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
      )}

      {tab === 'sheet' && (
        <>
          {!sheet ? (
            <p className="muted">Loading sheet…</p>
          ) : (
            <>
              <p className="muted">
                {sheet.groupCount} groups · {sheet.problemCount} items · structure from{' '}
                <a href={sheet.source} target="_blank" rel="noreferrer">
                  LearnYard DSA Sheet
                </a>
              </p>
              <input
                className="sheet-search"
                type="search"
                placeholder="Filter topics or problems…"
                value={query}
                onChange={(e) => setQuery(e.target.value)}
              />
              <div className="sheet-groups">
                {filteredGroups.map((g) => {
                  const open = openGroup === g.id || Boolean(query.trim())
                  const count = g.subgroups.reduce((n, sg) => n + sg.problems.length, 0)
                  return (
                    <details
                      key={g.id}
                      className="sheet-group"
                      open={open}
                      onToggle={(e) => {
                        if ((e.target as HTMLDetailsElement).open) setOpenGroup(g.id)
                      }}
                    >
                      <summary>
                        <span>{g.title}</span>
                        <span className="muted">{count}</span>
                      </summary>
                      {g.subgroups.map((sg) => (
                        <div key={sg.id} className="sheet-subgroup">
                          <h3>{sg.title}</h3>
                          <ul className="sheet-problems">
                            {sg.problems.map((p) => {
                              const href = problemHref(p)
                              return (
                                <li key={p.id}>
                                  <span className={`diff diff--${(p.difficulty || 'MEDIUM').toLowerCase()}`}>
                                    {p.difficulty}
                                  </span>
                                  {href ? (
                                    <a href={href} target="_blank" rel="noreferrer">
                                      {p.title}
                                    </a>
                                  ) : (
                                    <span>{p.title}</span>
                                  )}
                                </li>
                              )
                            })}
                          </ul>
                        </div>
                      ))}
                    </details>
                  )
                })}
              </div>
            </>
          )}
        </>
      )}
    </section>
  )
}
