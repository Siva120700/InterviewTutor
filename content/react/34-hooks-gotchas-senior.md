---
id: react-senior-gotchas
title: Senior React Gotchas and Design Review
track: react
module: "04 Senior Architecture"
order: 34
languages: [typescript]
summary: Stale closures, effect overuse, composition APIs, and how seniors review React PRs.
---

## Why this matters

This is the “senior signal” lesson — catch subtle bugs and guide team design.

## Definitions

- **Senior React judgment:** Optimizing for clear state ownership, minimal effects, accessible UI, and recoverable failures—not clever hooks.
- **Stale closure:** An effect/handler captured old props/state because deps were wrong, missing, or intentionally omitted.
- **Effect overuse:** Using `useEffect` to sync React state that should be derived during render or handled in events.
- **You Might Not Need an Effect:** Official guidance—derive values, respond in event handlers, or reset with `key` instead of mirroring props.
- **Composition root:** Where providers and app wiring live; keep leaf UI free of infrastructure setup.
- **Design review:** Evaluating API surface, state ownership, a11y, and failure modes—not only visual polish.
- **Fetch race:** Ignoring abort/ignore flags so an older response overwrites newer UI state.


## Common gotchas

```tsx
// Bad: effect mirrors props into state
useEffect(() => setValue(propValue), [propValue]);

// Better: use propValue directly, or key={id} to reset

// Bad: setState in effect that always loops
useEffect(() => setItems(items.filter(f)), [items]);
```

## PR review checklist

1. Where does state live?  
2. Any effects that should be events?  
3. Keys correct?  
4. Loading/error/empty paths?  
5. a11y + keyboard?  
6. Cache keys / races for data?  
7. Bundle impact of new deps?

## Interview Q&A

- **Q:** How do you avoid prop drilling?
  **A:** Composition (`children`), context for sparse globals — not a store by default.
- **Q:** Class components still?
  **A:** Error boundaries / legacy; migrate opportunistically.
- **Q:** Favorite performance win?
  **A:** Usually state placement or list virtualization > random memo.

## Pitfalls

- Abstracting too early (hooks for one use site)  
- Silent catch in boundaries  
- “Business logic in JSX” unreadable trees

## 60-second answer

“As a senior I optimize for clear state ownership, minimal effects, solid failure UI, and accessible composition. I review React changes for races, keys, and unnecessary re-render pressure.”

## Further study

- [React: You Might Not Need an Effect](https://react.dev/learn/you-might-not-need-an-effect) — the senior checklist for effect removal.
- [React: Lifecycle of Reactive Effects](https://react.dev/learn/lifecycle-of-reactive-effects) — why deps and cleanup matter.
- [React: Thinking in React](https://react.dev/learn/thinking-in-react) — state ownership and component boundaries.
- [React: Escape Hatches](https://react.dev/learn/escape-hatches) — when effects are actually appropriate.

## Practice prompts

1. Refactor three unnecessary effects  
2. Review a PR with index keys + fetch races  
3. Propose folder structure for a mid-size SPA
