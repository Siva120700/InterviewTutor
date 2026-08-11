---
id: react-rendering-performance
title: Rendering and Performance
track: react
module: "03 Advanced"
order: 20
languages: [typescript]
summary: Reconciliation, re-renders, React.memo, virtualization, and senior performance playbooks.
---

## Why this matters

Seniors diagnose “why is this slow?” with a mental model of renders — not random memoization.

## Definitions

- **Render:** The component function runs and returns React elements describing the next UI.
- **Commit:** React applies the computed DOM updates from a completed render.
- **Re-render:** A component runs again because its state, props, or context changed (or an ancestor re-rendered).
- **React.memo:** Higher-order component that shallow-compares props and skips re-render when they are equal.
- **Virtualization:** Rendering only visible rows of a large list (e.g. windowing) to keep DOM work bounded.
- **Waterfall:** Sequential awaits that inflate load time; often fixed by parallel fetches or Suspense boundaries.
- **Profiling first:** Measure with React DevTools/profiler before blanket `useMemo`/`memo`—re-renders are normal.


## What triggers re-renders

1. Own state update  
2. Parent re-render (unless memoized + stable props)  
3. Context value change  
4. Hooks that subscribe to external stores

## Patterns

```tsx
const Row = memo(function Row({ item, onSelect }: Props) {
  return <li onClick={() => onSelect(item.id)}>{item.name}</li>;
});

// Prefer: move state down; pass stable callbacks; split context
```

**Before memo:**  
- Does the parent need this state?  
- Can children be pure and receive primitives?  
- Profile with React DevTools.

## Large lists

```tsx
// Conceptual — use a virtualizer
// Only ~20 DOM nodes for 10k rows
```

## Interview Q&A

- **Q:** Is re-render bad?
  **A:** Cheap re-renders are fine; expensive trees or huge DOM updates are the problem.
- **Q:** `memo` everywhere?
  **A:** No — cost of comparison + complexity; use where profiling shows wins.
- **Q:** Keys and performance?
  **A:** Wrong keys remount trees → lost state and extra work.

## Pitfalls

- Inline `style={{}}` / `onClick={() => …}` breaking memo children  
- Context for high-churn values  
- Premature `useMemo` noise

## 60-second answer

“I treat re-renders as normal, then profile. I reduce work by state placement, stable props, selective memo, and virtualization for big lists — not blanket optimization.”

## Further study

- [React: Render and Commit](https://react.dev/learn/render-and-commit) — render vs commit phases.
- [React: memo](https://react.dev/reference/react/memo) — when prop memoization helps.
- [React: Specifying a Dependency Array](https://react.dev/reference/react/useEffect#specifying-reactive-dependencies) — understanding updates that drive work.
- [React: Keeping Components Pure](https://react.dev/learn/keeping-components-pure) — purity as a performance and correctness foundation.

## Practice prompts

1. Find why a memo child still re-renders  
2. Virtualize a 5k-row table  
3. Split a hot context into auth vs theme
