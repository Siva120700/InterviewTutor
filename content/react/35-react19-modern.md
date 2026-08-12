---
id: react-19-modern
title: Modern React — Actions, use, and Transitions
track: react
module: "04 Senior Architecture"
order: 35
languages: [typescript]
summary: React 19-era mental model — Actions, use(), form helpers, and when to adopt new APIs.
---

## Why this matters

Interviewers increasingly ask what’s new since hooks. You don’t need every RFC memorized — you need accurate mental models and migration judgment.

## Definitions

- **Action:** A function that runs a transition-aware async update (often form submit / mutation).
- **useFormStatus / useActionState:** Hooks for pending state and action results around forms (ecosystem + React versions vary — know the idea).
- **`use(promise)`:** Read a promise/context from render; Suspense handles loading (rules differ from hooks).
- **Transition:** Marking an update as non-urgent so urgent input stays responsive (`useTransition`).
- **Optimistic UI:** Show the expected result immediately; reconcile when the server responds (`useOptimistic` idea).
- **Document metadata:** Frameworks/`<title>` APIs evolving — prefer framework support for SEO.

## useTransition (stable and still key)

```tsx
const [isPending, startTransition] = useTransition();

function onFilter(next: string) {
  setInput(next); // urgent
  startTransition(() => setFilter(next)); // heavy list update
}
```

## Optimistic sketch

```tsx
const [optimistic, addOptimistic] = useOptimistic(messages);

async function send(text: string) {
  addOptimistic([...optimistic, { text, pending: true }]);
  await api.send(text);
  // parent refreshes canonical messages
}
```

(Exact APIs depend on React version — describe behavior even if syntax shifts.)

## `use` — conceptual

```tsx
// Inside a component that can Suspend:
const data = use(fetchUser(id)); // not a hook — can be conditional in some cases
```

Prefer frameworks/routers that integrate Suspense data properly rather than inventing ad-hoc promise caches.

## Adoption judgment

| Adopt freely | Be cautious |
|--------------|-------------|
| `useTransition` for heavy UI | Rewriting all forms to Actions overnight |
| Suspense boundaries for routes | Custom `use(promise)` caches without deduping |
| Optimistic UX for chat/likes | Optimistic updates without rollback |

## Interview Q&A

- **Q:** Actions vs `useEffect` fetch?  
  **A:** Mutations/events belong in actions/handlers; effects are for syncing with external systems.
- **Q:** Do we still need React Query?  
  **A:** Often yes for cache keys, retries, dedupe — Actions don’t replace a full server-state library.
- **Q:** React 19 compiler?  
  **A:** Auto-memoization research/tooling — say you’d measure before deleting all `useMemo`.

## Pitfalls

- Quoting APIs from blogs without version context  
- Optimistic UI that can’t roll back  
- Nesting Suspense poorly so the whole page flashes

## 60-second answer

“Modern React pushes async UI into transitions, Suspense, and action-style mutations. I use transitions for non-urgent updates, optimistic UI carefully, and I still rely on solid server-state tools where caching matters.”

## Further study

- Concurrent/Suspense · Data fetching · Senior gotchas  
- [React blog / react.dev](https://react.dev/blog)

## Practice prompts

1. Filter a big list with `useTransition`  
2. Design rollback for failed optimistic like  
3. Explain `use` vs `useEffect` loading data
