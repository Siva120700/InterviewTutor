---
id: react-custom-hooks
title: Custom Hooks
track: react
module: "02 Hooks and Composition"
order: 12
languages: [typescript]
summary: Extract reusable stateful logic — the senior composition tool.
---

## Why this matters

Custom hooks replace HOCs/render-props for shared logic. Seniors design clean hook APIs.

## Definitions

- **Custom hook:** A function named `useX` that calls other hooks to reuse stateful logic across components.
- **Hook composition:** Building higher-level behavior by combining `useState`, `useEffect`, and other hooks behind one API.
- **Hook API design:** Return stable, clear tuples/objects that are hard to misuse and easy to type.
- **Separation of concerns:** Components render UI; custom hooks own subscriptions, timers, and shared stateful logic.
- **Reusable logic ≠ reusable UI:** Hooks share behavior; presentational markup stays in components (or shared UI primitives).
- **Naming convention:** The `use` prefix is required so lint rules can enforce the Rules of Hooks.
- **Testability:** Prefer hooks that accept inputs/return values over ones that hard-code globals when practical.


## Examples

```tsx
function useLocalStorage<T>(key: string, initial: T) {
  const [value, setValue] = useState<T>(() => {
    const raw = localStorage.getItem(key);
    return raw ? (JSON.parse(raw) as T) : initial;
  });

  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);

  return [value, setValue] as const;
}

function useMediaQuery(query: string) {
  const [matches, setMatches] = useState(() => window.matchMedia(query).matches);
  useEffect(() => {
    const mql = window.matchMedia(query);
    const onChange = () => setMatches(mql.matches);
    mql.addEventListener('change', onChange);
    return () => mql.removeEventListener('change', onChange);
  }, [query]);
  return matches;
}
```

## Interview Q&A

- **Q:** Hook vs utility function?
  **A:** If it needs hooks/state/lifecycle → custom hook; else plain function.
- **Q:** Testing hooks?
  **A:** `@testing-library/react` `renderHook`, or test via a tiny harness component.
- **Q:** Can hooks be conditional?
  **A:** No — violate Rules of Hooks; conditionally *use results*, not calls.

## Pitfalls

- Giant `useApp` god hook  
- Returning new object identities every render without need  
- Hiding too much → hard to debug

## 60-second answer

“Custom hooks extract reusable stateful logic behind a `use` API. I keep them focused, document return shapes, and leave presentational JSX in components.”

## Further study

- [React: Reusing Logic with Custom Hooks](https://react.dev/learn/reusing-logic-with-custom-hooks) — official custom-hook guidance.
- [React: Rules of Hooks](https://react.dev/reference/rules/rules-of-hooks) — why `use` naming and call order matter.
- [React: useEffect](https://react.dev/reference/react/useEffect) — cleanup patterns custom hooks often wrap.
- [React: Escape Hatches](https://react.dev/learn/escape-hatches) — when hooks sync with external systems.

## Practice prompts

1. `useFetch` with abort + loading/error  
2. `useDebouncedValue`  
3. Refactor duplicated subscription code into a hook
