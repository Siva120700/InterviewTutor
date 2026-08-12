---
id: react-hooks-recipes
title: Custom Hook Recipes
track: react
module: "02 Hooks and Composition"
order: 18
languages: [typescript]
summary: Reusable hook patterns — debounce, previous, toggle, localStorage, media query, interval.
---

## Why this matters

Interviews often ask you to write a small custom hook. These recipes are the usual suspects — know definitions and clean implementations.

## Definitions

- **Custom hook:** Function starting with `use` that composes built-in hooks to share stateful logic.
- **Debounce:** Delay updating a value until input has been quiet for N ms.
- **Previous value:** Ref-captured value from the last render.
- **Toggle:** Boolean state with a stable `toggle` helper.
- **Synced storage:** State mirrored to `localStorage` / `sessionStorage`.
- **Declarative interval:** Interval that reflects the latest callback without resubscribing every render.

## usePrevious

```tsx
export function usePrevious<T>(value: T): T | undefined {
  const ref = useRef<T>();
  useEffect(() => {
    ref.current = value;
  }, [value]);
  return ref.current;
}
```

## useToggle

```tsx
export function useToggle(initial = false) {
  const [on, setOn] = useState(initial);
  const toggle = useCallback(() => setOn((v) => !v), []);
  const setTrue = useCallback(() => setOn(true), []);
  const setFalse = useCallback(() => setOn(false), []);
  return { on, toggle, setTrue, setFalse } as const;
}
```

## useDebouncedValue

```tsx
export function useDebouncedValue<T>(value: T, ms: number): T {
  const [debounced, setDebounced] = useState(value);
  useEffect(() => {
    const t = setTimeout(() => setDebounced(value), ms);
    return () => clearTimeout(t);
  }, [value, ms]);
  return debounced;
}
```

## useLocalStorage

```tsx
export function useLocalStorage<T>(key: string, initial: T) {
  const [value, setValue] = useState<T>(() => {
    try {
      const raw = localStorage.getItem(key);
      return raw != null ? (JSON.parse(raw) as T) : initial;
    } catch {
      return initial;
    }
  });

  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);

  return [value, setValue] as const;
}
```

## useMediaQuery

```tsx
export function useMediaQuery(query: string): boolean {
  return useSyncExternalStore(
    (onChange) => {
      const m = window.matchMedia(query);
      m.addEventListener('change', onChange);
      return () => m.removeEventListener('change', onChange);
    },
    () => window.matchMedia(query).matches,
    () => false,
  );
}
```

## useInterval

```tsx
export function useInterval(fn: () => void, ms: number | null) {
  const fnRef = useRef(fn);
  useEffect(() => {
    fnRef.current = fn;
  }, [fn]);

  useEffect(() => {
    if (ms == null) return;
    const id = setInterval(() => fnRef.current(), ms);
    return () => clearInterval(id);
  }, [ms]);
}
```

Pattern: **latest callback in a ref** so the interval isn’t reset every render.

## useOnClickOutside

```tsx
export function useOnClickOutside(
  ref: React.RefObject<HTMLElement | null>,
  handler: () => void,
) {
  useEffect(() => {
    function onPointer(e: MouseEvent | TouchEvent) {
      const el = ref.current;
      if (!el || el.contains(e.target as Node)) return;
      handler();
    }
    document.addEventListener('mousedown', onPointer);
    document.addEventListener('touchstart', onPointer);
    return () => {
      document.removeEventListener('mousedown', onPointer);
      document.removeEventListener('touchstart', onPointer);
    };
  }, [ref, handler]);
}
```

Stabilize `handler` with `useCallback` in the caller.

## Interview Q&A

- **Q:** What makes a good custom hook API?  
  **A:** Small, named for the feature (`useCart`), returns tuple/object consistently, documents side effects.
- **Q:** Hook vs component?  
  **A:** Hook = reusable **logic**; component = reusable **UI**.
- **Q:** Testing hooks?  
  **A:** `renderHook` from Testing Library; assert returned values and state transitions.

## Pitfalls

- Forgetting cleanup in timeout/interval/listener hooks  
- Debouncing the wrong thing (debounce the fetch, or the value — be explicit)  
- JSON-parsing storage without try/catch  

## 60-second answer

“Custom hooks package stateful logic behind a `use*` API. I keep them focused, clean up subscriptions, and use refs when I need the latest callback without restarting effects.”

## Further study

- Custom Hooks lesson · Hooks Dictionary · Machine coding  

## Practice prompts

1. Write `useFetch(url)` with abort + status  
2. Write `useWindowSize` via `useSyncExternalStore`  
3. Combine debounce + fetch for typeahead
