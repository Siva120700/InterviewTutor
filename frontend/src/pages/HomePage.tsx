import { Link } from 'react-router-dom'

export function HomePage() {
  return (
    <section className="hero">
      <p className="eyebrow">Personal prep desk</p>
      <h1 className="hero-title">InterviewTutor</h1>
      <p className="lede">
        DSA, LLD, HLD, CS fundamentals, Java, and .NET — with an AI doubt chat on every lesson.
      </p>
      <div className="cta-row">
        <Link className="btn" to="/tracks">
          Open tracks
        </Link>
        <Link className="btn btn--ghost" to="/mock">
          Timed mock
        </Link>
      </div>
    </section>
  )
}
