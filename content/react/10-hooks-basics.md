---
id: react-hooks-basics
title: Hooks Basics — useState and useEffect
track: react
module: "02 Hooks and Composition"
order: 10
languages: [typescript]
summary: Rules of hooks, state updates, effect lifecycle, cleanup, and dependency arrays.
---

## Why this matters

Hooks are the modern React API. Seniors must explain effects, deps, and cleanup without cargo-culting.

## Definitions

- **Hook:** A function whose name starts with `use` that lets function components use React features (state, effects, context).
- **Rules of Hooks:** Call hooks only at the top level of React functions, in the same order every render—no conditional/loop calls.
- **useState:** Declares state and a setter; calling the setter schedules a re-render with the new value.
- **useEffect:** Runs side effects after paint to sync with external systems; may return a cleanup function.
- **Dependency array:** Values the effect depends on; React re-runs the effect when any dependency changes (`Object.is`).
- **Cleanup:** Function returned from an effect that undoes subscriptions/timers/aborts before re-run or unmount.
- **Stale closure:** Effect or handler capturing old props/state because dependencies were incomplete or wrong.
- **Batching:** React groups multiple state updates in one event/task into a single re-render.
- **Lazy initialization:** Passing `useState(() => initial)` so expensive setup runs once on mount only.


## useState

```tsx
const [query, setQuery] = useState('');
setQuery('react');           // replace
setQuery(q => q.trim());     // functional update from previous

// Lazy init — function runs once
const [data, setData] = useState(() => createExpensiveIndex(items));

// Object / array state — replace immutably
setUser(u => ({ ...u, name: 'Ada' }));
setItems(list => [...list, item]);
```

### Derived state (no effect)

```tsx
// Good — compute during render
const fullName = `${first} ${last}`;
const visible = items.filter(i => i.active);

// Bad — mirror props/state in an effect
// useEffect(() => setFullName(`${first} ${last}`), [first, last]);
```

## useEffect

```tsx
useEffect(() => {
  const controller = new AbortController();
  fetch(`/api/search?q=${encodeURIComponent(query)}`, { signal: controller.signal })
    .then(r => r.json())
    .then(setResults)
    .catch(err => {
      if (err.name !== 'AbortError') setError(err);
    });
  return () => controller.abort();
}, [query]);
```

| Deps | Behavior |
|------|----------|
| omitted | Runs after **every** render — rarely what you want |
| `[]` | mount (+ strict remount in dev) |
| `[a,b]` | when a/b change |
| forgotten deps | stale closures / bugs |

See **Hooks Dictionary** for the full list of built-in hooks and definitions.

## Interview Q&A

- **Q:** Why cleanup?
  **A:** Avoid leaks and race conditions (stale responses updating unmounted/new state).
- **Q:** Effect vs event handler?
  **A:** Effects sync with external systems after render; user clicks belong in handlers.
- **Q:** Strict Mode double-invoke?
  **A:** Dev-only to surface missing cleanup; design effects idempotent.

## Pitfalls

- Fetch without abort → race  
- Putting objects/functions in deps without memo → infinite loops  
- Using effects to compute derived state

## 60-second answer

“useState holds local state; useEffect syncs with the outside world after render with explicit deps and cleanup. I keep render pure and put user interactions in handlers.”

## Further study

- [React: Built-in React Hooks](https://react.dev/reference/react) — hook API reference map.
- [React: useState](https://react.dev/reference/react/useState) — state updates and batching.
- [React: useEffect](https://react.dev/reference/react/useEffect) — effects, deps, and cleanup.
- [React: You Might Not Need an Effect](https://react.dev/learn/you-might-not-need-an-effect) — when to prefer events or derived state.

## Practice prompts

1. Debounced search with abort  
2. Subscribe to `resize` with cleanup  
3. Fix a stale closure bug in an interval
