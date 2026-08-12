---
id: react-routing
title: Client Routing with React Router
track: react
module: "02 Hooks and Composition"
order: 13
languages: [typescript]
summary: SPA routing, nested layouts, loaders/actions mental model, and protected routes.
---

## Why this matters

Almost every product React app is a multi-route SPA or hybrid. Interviews expect nested layouts, params, and auth gates — not only `<a href>`.

## Definitions

- **Client-side routing:** Updating URL and UI without full page reloads via the History API.
- **Route:** A URL pattern mapped to a component tree.
- **Nested routes / layout routes:** Parent route renders an outlet; children fill it.
- **Dynamic segment:** Param like `/users/:id` → `useParams()`.
- **Link vs anchor:** `<Link>` (or framework `<Link>`) navigates client-side; raw `<a>` may hard-reload.
- **Protected route:** Wrapper that redirects unauthenticated users.
- **Loader/action (RR 6.4+ / frameworks):** Data APIs colocated with routes — know the idea even if you use Next.

## Basic routes

```tsx
import { BrowserRouter, Routes, Route, Link, Outlet, useParams, Navigate } from 'react-router-dom';

function AppShell() {
  return (
    <>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/users/42">User</Link>
      </nav>
      <Outlet />
    </>
  );
}

function UserPage() {
  const { id } = useParams();
  return <h1>User {id}</h1>;
}

function Private({ children }: { children: React.ReactNode }) {
  const authed = true; // from auth context
  return authed ? children : <Navigate to="/login" replace />;
}

export function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppShell />}>
          <Route index element={<Home />} />
          <Route path="users/:id" element={<UserPage />} />
          <Route
            path="settings"
            element={
              <Private>
                <Settings />
              </Private>
            }
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
```

## Nested UI mental model

```text
/app                → AppLayout
/app/orders         → AppLayout + OrdersPage
/app/orders/:id     → AppLayout + OrderDetail
```

Parent keeps chrome (nav/sidebar); child swaps in `<Outlet />`.

## Search params

```tsx
import { useSearchParams } from 'react-router-dom';

const [params, setParams] = useSearchParams();
const q = params.get('q') ?? '';
setParams({ q: 'react' }); // updates URL
```

Prefer URL for shareable filter/sort state.

## Interview Q&A

- **Q:** React Router vs Next.js routing?  
  **A:** RR is client library for CSR SPAs; Next owns file-based routing, SSR/RSC. Concepts (layouts, params) transfer.
- **Q:** Where should auth redirect live?  
  **A:** Route guard / loader / middleware — not deep inside every leaf button.
- **Q:** Why `replace` on login redirect?  
  **A:** Avoid back-button returning to the login form loop.

## Pitfalls

- Nesting `BrowserRouter` twice  
- Using `href="/x"` everywhere and losing SPA transitions  
- Putting fetch-only-on-mount logic that breaks on param change without deps/`key`

## 60-second answer

“I map URL segments to nested layouts with an outlet. Params and search params hold shareable state. Auth uses a guard that redirects, and I distinguish client routers from full-stack frameworks like Next.”

## Further study

- SSR/Next architecture lesson  
- [React Router docs](https://reactrouter.com/)

## Practice prompts

1. Nested layout with sidebar + two child pages  
2. Protected route with redirect-to-login preserving `from`  
3. Sync a filter dropdown to `?status=`
