---
id: react-hooks-dictionary
title: Hooks Dictionary — Every Built-in Hook
track: react
module: "02 Hooks and Composition"
order: 9
languages: [typescript]
summary: Definitions, signatures, and when to use every common React hook — interview cheat sheet.
---

## Why this matters

Interviews ask “what does X hook do?” under time pressure. This lesson is the one-page map of built-in hooks with precise definitions.

## Rules of Hooks (applies to all)

1. Only call hooks at the **top level** of a React function component or custom hook.  
2. Only call hooks from **React functions** (not plain JS helpers).  
3. Call them in the **same order** every render — no `if` / loops / early returns before hooks.

**Why:** React associates hook state with call order on the fiber.

---

## State hooks

### `useState`

**Definition:** Declares a state variable and a setter; updating state queues a re-render.

```tsx
const [value, setValue] = useState(initial);
const [value, setValue] = useState(() => expensiveInit()); // lazy init
setValue(next);
setValue(prev => prev + 1); // functional update
```

| Use when | Avoid when |
|----------|------------|
| UI must update when data changes | You only need a mutable box (use `useRef`) |
| Value is used in render | Syncing with external store (prefer `useSyncExternalStore`) |

---

### `useReducer`

**Definition:** State update via `(state, action) => nextState` — better for complex transitions or shared update logic.

```tsx
type State = { count: number };
type Action = { type: 'inc' } | { type: 'add'; n: number };

function reducer(state: State, action: Action): State {
  switch (action.type) {
    case 'inc': return { count: state.count + 1 };
    case 'add': return { count: state.count + action.n };
  }
}

const [state, dispatch] = useReducer(reducer, { count: 0 });
dispatch({ type: 'inc' });
```

**Use when:** many related fields, next state depends on previous in structured ways, or you want testable update logic.

---

## Effect hooks

### `useEffect`

**Definition:** Schedule a **side effect after paint** to sync React with an external system; optional cleanup runs before re-run/unmount.

```tsx
useEffect(() => {
  const id = setInterval(tick, 1000);
  return () => clearInterval(id);
}, [tick]);
```

**External systems:** network, DOM subscriptions, timers, non-React widgets — **not** derived state.

---

### `useLayoutEffect`

**Definition:** Like `useEffect`, but runs **after DOM updates, before the browser paints** — use for measuring/mutating DOM that must not flash.

```tsx
useLayoutEffect(() => {
  const rect = ref.current?.getBoundingClientRect();
  // set position before paint to avoid flicker
}, [deps]);
```

**Prefer `useEffect`** unless you have a visible flicker / measurement need. On the server, `useLayoutEffect` warns — gate or use `useEffect`.

---

### `useInsertionEffect`

**Definition:** Fires **before** layout effects — intended for CSS-in-JS libraries injecting `<style>` tags. App code rarely needs it.

---

## Ref hooks

### `useRef`

**Definition:** Persistent mutable `{ current }` that does **not** trigger re-render when changed.

```tsx
const ref = useRef<HTMLInputElement>(null);
const renders = useRef(0);
renders.current++;
```

**Use for:** DOM nodes, timer IDs, previous values, imperative handles.

---

### `useImperativeHandle`

**Definition:** Customizes the instance value exposed to parent components via `ref` (with `forwardRef`).

```tsx
useImperativeHandle(ref, () => ({ focus: () => inputRef.current?.focus() }), []);
```

Keep the exposed API **small**.

---

## Context hook

### `useContext`

**Definition:** Reads the nearest `Context.Provider` value and subscribes the component to updates.

```tsx
const theme = useContext(ThemeContext);
```

All consumers re-render when the provider **value** identity/content changes.

---

## Performance hooks

### `useMemo`

**Definition:** Caches a **computed value** until dependencies change (`Object.is`).

```tsx
const sorted = useMemo(() => [...items].sort(cmp), [items]);
```

---

### `useCallback`

**Definition:** Caches a **function identity** until dependencies change — sugar over `useMemo(() => fn, deps)`.

```tsx
const onSave = useCallback(() => save(id), [id]);
```

**Use when:** passing callbacks to `memo` children or as effect deps that would otherwise churn.

---

### `useTransition`

**Definition:** Marks a state update as a **non-urgent transition** so urgent updates (typing) stay responsive.

```tsx
const [isPending, startTransition] = useTransition();
startTransition(() => setHeavyFilter(next));
```

---

### `useDeferredValue`

**Definition:** Returns a **deferred** version of a value that may lag behind — useful for expensive children.

```tsx
const deferredQuery = useDeferredValue(query);
return <Results query={deferredQuery} />;
```

Similar goal to transitions; often simpler when you don’t control the setter.

---

## Identity & external store

### `useId`

**Definition:** Generates a unique **stable ID** string for a11y attributes (`htmlFor` / `aria-*`), SSR-safe.

```tsx
const id = useId();
return (
  <>
    <label htmlFor={id}>Email</label>
    <input id={id} />
  </>
);
```

Not for list keys.

---

### `useSyncExternalStore`

**Definition:** Subscribe to an **external store** (Redux, browser API) with correct concurrent/SSR support.

```tsx
const width = useSyncExternalStore(
  (onStoreChange) => {
    window.addEventListener('resize', onStoreChange);
    return () => window.removeEventListener('resize', onStoreChange);
  },
  () => window.innerWidth,
  () => 0, // server snapshot
);
```

Prefer this over ad-hoc `useEffect` + `useState` for external stores.

---

## Modern / Suspense-related

### `use` (React 19+)

**Definition:** Read a **Promise** or **Context** during render; Promises integrate with Suspense. Not a hook — can be called conditionally in supported cases.

```tsx
const data = use(resourcePromise);
const theme = use(ThemeContext);
```

---

### `useOptimistic` (React 19+)

**Definition:** Show an **optimistic** UI state while an async action is in flight, then reconcile.

---

### `useActionState` / form Actions (React 19+)

**Definition:** Track result + pending state of an **Action** (async transition function), often with forms.

Know the *idea* even if your project’s exact API names differ by version.

---

## Form status (React 19+)

### `useFormStatus`

**Definition:** Read pending status of a parent `<form>` action from a child component (e.g. disable submit button).

---

## Quick decision table

| Need | Hook |
|------|------|
| Value in UI | `useState` / `useReducer` |
| Sync with outside world | `useEffect` |
| Measure DOM before paint | `useLayoutEffect` |
| DOM node / mutable box | `useRef` |
| Shared subtree data | `useContext` |
| Expensive pure calc | `useMemo` |
| Stable callback | `useCallback` |
| Keep input snappy under heavy UI | `useTransition` / `useDeferredValue` |
| SSR-safe unique id | `useId` |
| Subscribe to Redux/browser store | `useSyncExternalStore` |
| Optimistic mutation UI | `useOptimistic` |

## Interview Q&A

- **Q:** `useEffect` vs `useLayoutEffect`?  
  **A:** Effect after paint; layout effect before paint for DOM measurement/sync.
- **Q:** `useMemo` vs `useCallback`?  
  **A:** Memo caches values; callback caches functions (same mechanism).
- **Q:** Why not put hooks in `if`?  
  **A:** Breaks call-order association → wrong state on later renders.

## Pitfalls

- Using `useEffect` to compute derived state  
- `useMemo`/`useCallback` everywhere without measurement  
- `useId` for list keys  
- External store via `useEffect` instead of `useSyncExternalStore`

## 60-second answer

“Hooks are the `use*` API for state, effects, refs, and context in function components. I pick `useState`/`useReducer` for UI state, `useEffect` for external sync, refs for non-rendering mutables, and the concurrent hooks when interactions must stay responsive.”

## Further study

- Hooks Basics · Hooks Advanced · Custom Hooks · Modern React  
- [Built-in React Hooks](https://react.dev/reference/react/hooks)

## Practice prompts

1. Recite rules of hooks + why  
2. Pick a hook for: window width, expensive sort, login user, input focus  
3. Rewrite an effect-based store subscription with `useSyncExternalStore`
