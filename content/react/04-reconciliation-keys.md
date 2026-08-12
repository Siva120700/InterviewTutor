---
id: react-reconciliation-keys
title: Virtual DOM, Reconciliation, and Keys
track: react
module: "01 Foundations"
order: 4
languages: [typescript]
summary: How React diffs trees, why keys matter, and what “virtual DOM” actually means in interviews.
---

## Why this matters

“How does React update the UI?” is a classic screen question. Seniors must connect keys, identity, and re-render cost without hand-waving “virtual DOM is faster.”

## Definitions

- **Virtual DOM:** An in-memory tree of React elements describing UI — not a secret faster DOM, a diffing input.
- **Reconciliation:** React’s algorithm to compare previous and next trees and apply minimal host updates (DOM).
- **Fiber:** React’s internal unit of work enabling incremental rendering and prioritization (concurrent features).
- **Key:** A stable identity for a child among siblings so React can match list items across renders.
- **Host instance:** The real DOM node (or native view) tied to a fiber.
- **Bail out:** Skipping deeper work when props/state are unchanged (e.g., `memo`, same state reference).

## Concept

```text
State change → render (JSX → elements) → reconcile vs previous tree → commit DOM mutations
```

React does **not** magically skip all work. Render still runs (unless you bail out). Reconciliation decides *which DOM ops* happen.

## Keys — correct vs wrong

```tsx
// Bad: index as key when list can reorder/insert
{todos.map((t, i) => <TodoRow key={i} todo={t} />)}

// Good: stable id from data
{todos.map((t) => <TodoRow key={t.id} todo={t} />)}
```

Wrong keys cause:
- Wrong component state attached to the wrong row  
- Input focus jumping  
- Extra unmount/remount (lost state, extra effects)

## Resetting state with `key`

```tsx
// Changing key remounts the editor — clean reset
<Editor key={docId} docId={docId} />
```

Prefer this over `useEffect(() => setDraft(''), [docId])` when you want a full reset.

## Interview Q&A

- **Q:** Is virtual DOM always faster than vanilla DOM?  
  **A:** No. It optimizes *developer model* and batches updates; careful vanilla can be faster for tiny cases.
- **Q:** Why not use index as key?  
  **A:** Fine for static lists never reordered; unsafe when insert/delete/reorder with stateful children.
- **Q:** Diffing is O(n³)?  
  **A:** Naive tree diff is expensive; React uses heuristics (same component type at position, keys for lists) for O(n) practical behavior.

## Pitfalls

- Random keys (`key={Math.random()}`) → remount every render  
- Using key to “force update” as a habit instead of fixing state  
- Confusing re-render (JS ran) with re-paint (DOM changed)

## 60-second answer

“React renders a description of UI, reconciles it with the previous tree, then commits DOM changes. Keys give list children stable identity so state follows the right item. Virtual DOM is the element tree, not a magic performance guarantee.”

## Further study

- Rendering & Performance lesson  
- [Reconciliation (React docs)](https://react.dev/learn/preserving-and-resetting-state)

## Practice prompts

1. Demo a buggy todo list using index keys with inputs  
2. Explain fiber in two sentences  
3. When is remount-via-`key` the right API?
