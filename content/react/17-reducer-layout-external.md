---
id: react-reducer-layout-external
title: useReducer, Layout Effects, and External Stores
track: react
module: "02 Hooks and Composition"
order: 17
languages: [typescript]
summary: Deep dive on useReducer, useLayoutEffect, useId, useDeferredValue, and useSyncExternalStore.
---

## Why this matters

Beyond `useState`/`useEffect`, seniors are expected to pick `useReducer` for complex state, layout effects for flicker-free DOM work, and `useSyncExternalStore` for correct external subscriptions.

## Definitions

- **Reducer:** Pure `(state, action) => nextState` function — same idea as Redux reducers.
- **Dispatch:** Function that sends an action into the reducer; identity is stable.
- **useLayoutEffect:** Effect that runs after DOM mutations but before paint.
- **useId:** Stable unique ID for a11y wiring across SSR/client.
- **useDeferredValue:** Deferred mirror of a changing value for low-priority rendering.
- **useSyncExternalStore:** Concurrent-safe subscribe/getSnapshot API for non-React data sources.
- **Tear:** In concurrent rendering, reading a mutable external store inconsistently mid-render — what `useSyncExternalStore` prevents.

## useReducer — when it shines

```tsx
type State = { status: 'idle' | 'loading' | 'error' | 'success'; data?: User; error?: string };
type Action =
  | { type: 'fetch' }
  | { type: 'ok'; data: User }
  | { type: 'fail'; error: string }
  | { type: 'reset' };

function reducer(state: State, action: Action): State {
  switch (action.type) {
    case 'fetch': return { status: 'loading' };
    case 'ok': return { status: 'success', data: action.data };
    case 'fail': return { status: 'error', error: action.error };
    case 'reset': return { status: 'idle' };
  }
}

export function useUserLoader() {
  const [state, dispatch] = useReducer(reducer, { status: 'idle' });
  // dispatch({ type: 'fetch' }) …
  return { state, dispatch };
}
```

**vs useState:** Prefer reducer when updates are multi-field, event-driven, or easier to test as a pure function.

### Lazy init

```tsx
const [state, dispatch] = useReducer(reducer, userId, (id) => loadCached(id));
```

Third argument is an init function receiving the initial arg.

## useLayoutEffect — measure then place

```tsx
const ref = useRef<HTMLDivElement>(null);
const [height, setHeight] = useState(0);

useLayoutEffect(() => {
  if (!ref.current) return;
  setHeight(ref.current.getBoundingClientRect().height);
}, [content]);
```

Use sparingly — it blocks paint. Prefer CSS or `useEffect` when flicker is acceptable.

## useId — labels and a11y

```tsx
function Field({ label }: { label: string }) {
  const id = useId();
  return (
    <>
      <label htmlFor={id}>{label}</label>
      <input id={id} aria-describedby={`${id}-hint`} />
      <p id={`${id}-hint`}>Hint</p>
    </>
  );
}
```

## useDeferredValue — snappy typing

```tsx
function Search({ items }: { items: Item[] }) {
  const [query, setQuery] = useState('');
  const deferred = useDeferredValue(query);
  const filtered = useMemo(
    () => items.filter((i) => i.name.includes(deferred)),
    [items, deferred],
  );
  const stale = query !== deferred;

  return (
    <>
      <input value={query} onChange={(e) => setQuery(e.target.value)} />
      <div style={{ opacity: stale ? 0.7 : 1 }}>
        <List items={filtered} />
      </div>
    </>
  );
}
```

## useSyncExternalStore — browser / Redux

```tsx
function subscribeOnline(cb: () => void) {
  window.addEventListener('online', cb);
  window.addEventListener('offline', cb);
  return () => {
    window.removeEventListener('online', cb);
    window.removeEventListener('offline', cb);
  };
}

export function useOnline() {
  return useSyncExternalStore(
    subscribeOnline,
    () => navigator.onLine,
    () => true, // SSR assume online
  );
}
```

For Redux-like stores: `subscribe`, `getState` as snapshot, and a server snapshot when SSR.

## Interview Q&A

- **Q:** Is dispatch safe to omit from deps?  
  **A:** Yes — React guarantees `dispatch` is stable.
- **Q:** Can I always replace Redux with useReducer?  
  **A:** Local/complex component state yes; cross-app shared cache often needs a store or server-state library.
- **Q:** useDeferredValue vs useTransition?  
  **A:** Transition wraps the **setState**; deferred wraps a **value** you already have.

## Pitfalls

- Layout effects that setState every time → render loops  
- Reading `Date.now()` or `Math.random()` in render instead of store snapshot discipline  
- Using `useId` for list keys  

## 60-second answer

“useReducer structures complex updates as pure actions. useLayoutEffect measures DOM before paint. useDeferredValue keeps heavy UI behind fast input. useSyncExternalStore is the correct way to subscribe to external data under concurrent rendering.”

## Further study

- Hooks Dictionary · Concurrent/Suspense · State management  
- [useSyncExternalStore](https://react.dev/reference/react/useSyncExternalStore)

## Practice prompts

1. Refactor multi-`useState` form wizard to `useReducer`  
2. Build `useMediaQuery('(min-width: 800px)')` with `useSyncExternalStore`  
3. Tooltip positioning with `useLayoutEffect`
