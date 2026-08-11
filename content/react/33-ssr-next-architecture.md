---
id: react-ssr-next
title: SSR, Next.js, and Frontend Architecture
track: react
module: "04 Senior Architecture"
order: 33
languages: [typescript]
summary: CSR vs SSR vs RSC, Next.js mental model, bundling, and senior frontend architecture.
---

## Why this matters

Senior FS roles expect you to place React in a real architecture — routing, rendering strategy, and boundaries.

## Definitions

- **CSR (Client-Side Rendering):** Browser downloads JS then renders—great for apps; weaker SEO/first paint without help.
- **SSR (Server-Side Rendering):** HTML generated on the server per request for faster first content and better SEO.
- **SSG / ISR:** Pre-render at build time (SSG) or regenerate on an interval/on-demand (ISR).
- **RSC (React Server Components):** Components that run on the server by default and ship less client JS for that tree.
- **Hydration:** Client React attaches event handlers and state to server-rendered HTML.
- **Client boundary:** Explicit `'use client'` split—interactive islands vs server data/auth walls.
- **Hydration mismatch:** Server HTML differs from the client’s first render (time, random ids, browser-only APIs).


## When to pick what

| Goal | Approach |
|------|----------|
| SEO marketing pages | SSR/SSG |
| Auth dashboards | CSR or hybrid |
| Reduce JS | RSC + small client islands |
| Highly interactive editors | Client components |

## Architecture sketch

```text
app/
  (marketing)/ page.tsx      — server
  dashboard/ page.tsx        — server shell
  dashboard/widget.tsx       — 'use client' for interactivity
lib/api.ts                   — server fetch
```

## Performance & security talking points

- Code-split routes; analyze bundle  
- Don’t leak server secrets to client components  
- Auth checks on server for protected data  
- Streaming + Suspense for TTFB UX

## Interview Q&A

- **Q:** Hydration mismatch?
  **A:** Server HTML ≠ client first render (random ids, `Date.now`, locale). Keep first render deterministic.
- **Q:** Why not all client?
  **A:** Bundle size, SEO, data secrecy, waterfalls.
- **Q:** Microfrontends?
  **A:** Org-scale trade-off — isolation vs complexity; mention Module Federation carefully.

## Pitfalls

- Marking everything `'use client'`  
- Fetching secrets in client  
- Ignoring error/loading UI per route segment

## 60-second answer

“I choose CSR/SSR/RSC based on SEO, interactivity, and data sensitivity. Next-style apps keep server by default, push client islands where needed, and enforce auth on the server.”

## Further study

- [Next.js documentation](https://nextjs.org/docs) — App Router, SSR/SSG, and deployment model.
- [React: Server Components](https://react.dev/reference/rsc/server-components) — RSC mental model and constraints.
- [Next.js: Rendering](https://nextjs.org/docs/app/building-your-application/rendering) — CSR/SSR/SSG/RSC choices.
- [React: Streaming](https://react.dev/reference/react-dom/server/renderToPipeableStream) — streaming SSR building blocks.

## Practice prompts

1. Design a blog + dashboard hybrid rendering strategy  
2. Explain a hydration mismatch root cause  
3. Draw trust boundaries for a BFF + React app
