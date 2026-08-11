import { NavLink, Outlet } from 'react-router-dom'

export function Layout() {
  return (
    <div className="shell">
      <header className="top">
        <NavLink to="/" className="brand">
          InterviewTutor
        </NavLink>
        <nav className="nav">
          <NavLink to="/tracks">Tracks</NavLink>
          <NavLink to="/practice">Practice</NavLink>
          <NavLink to="/mock">Mock</NavLink>
          <NavLink to="/progress">Progress</NavLink>
        </nav>
      </header>
      <main className="main">
        <Outlet />
      </main>
    </div>
  )
}
