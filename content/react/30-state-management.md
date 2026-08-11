---
id: react-state-management
title: State Management at Scale
track: react
module: "04 Senior Architecture"
order: 30
languages: [typescript]
summary: Local vs server vs global state — Context, Redux/Zustand, and senior decision frameworks.
---

## Why this matters

Seniors are judged on *choosing* state tools, not memorizing Redux boilerplate.

## Definitions

- **State management:** Choosing where data lives and how it updates—local UI, server cache, or shared client store.
- **Local UI state:** Owned by a component (toggles, draft inputs)—default to `useState` / `useReducer`.
- **Server state:** Remote data with caching, staleness, and mutations (TanStack Query, SWR, RTK Query).
- **Global client state:** Cross-cutting client-only state (feature flags, wizard step, UI shell)—keep it thin.
- **Flux / Redux:** Unidirectional store with actions → reducer → state; useful for complex client workflows.
- **Zustand / Jotai:** Lightweight stores or atoms with less ceremony than classic Redux.
- **Single source of truth:** One authoritative place per piece of data—avoid mirroring server data into Redux by default.


## Decision framework

| Kind | Default tool |
|------|----------------|
| Form / widget | `useState` / `useReducer` |
| Shared in a feature | Lift state or feature context |
| Remote lists/details | Server-state library |
| App-wide rare writes | Small store or context |

```tsx
// Server state example (TanStack Query sketch)
const { data, isLoading, error } = useQuery({
  queryKey: ['todos'],
  queryFn: fetchTodos,
});
```

## Redux when?

- Complex client workflows, time-travel/debug needs, broad middleware ecosystem  
- Otherwise prefer simpler stores + server cache

## Interview Q&A

- **Q:** Context vs Redux?
  **A:** Context is DI; not optimized as a high-frequency store. Redux/Zustand handle updates/selectors better.
- **Q:** Duplicate server data in Redux?
  **A:** Usually avoid — use a server cache library.
- **Q:** useReducer vs Redux?
  **A:** useReducer is local; Redux is app-wide with tooling.

## Pitfalls

- Putting all server data in Redux “because enterprise”  
- One mega-context for the whole app  
- Prop drilling fear → premature global store

## 60-second answer

“I separate local, server, and global state. Most ‘async data’ belongs in a server cache. Global stores stay thin for true client concerns.”

## Further study

- [React: Managing State](https://react.dev/learn/managing-state) — official decision framework for where state lives.
- [TanStack Query: Overview](https://tanstack.com/query/latest/docs/framework/react/overview) — server-state caching model.
- [Redux Toolkit documentation](https://redux-toolkit.js.org/introduction/getting-started) — modern Redux when you need a client store.
- [React: Scaling Up with Reducer and Context](https://react.dev/learn/scaling-up-with-reducer-and-context) — medium-complexity shared client state.

## Practice prompts

1. Classify 10 pieces of state in an e-commerce app  
2. Replace manual fetch+useEffect with React Query  
3. Design a cart store API (add/remove/persist)
