---
id: react-hooks-advanced
title: useRef, useMemo, useCallback, useContext
track: react
module: "02 Hooks"
order: 11
languages: [typescript]
summary: Refs vs state, memoization when it matters, and context for cross-tree data.
---

## Why this matters

Senior interviews probe whether you *overuse* memoization or misuse refs. Know the right tool.

## Definitions

- **useRef:** Mutable box (`{ current }`) that persists across renders without causing a re-render when updated.
- **useMemo:** Memoizes an expensive computed value until its dependency list changes.
- **useCallback:** Memoizes a function identity until deps change—useful for stable props to memoized children.
- **useContext:** Reads the nearest matching Provider value and re-renders when that value changes.
- **Context:** React’s tree-scoped dependency injection for shared data (theme, auth)—not automatically a global store.
- **Referential equality:** `Object.is` / `===` identity; new object/function literals each render defeat memoization.
- **React Compiler:** Tooling that can auto-memoize; interviews still expect you to understand manual memo trade-offs.


## useRef

```tsx
const inputRef = useRef<HTMLInputElement>(null);
useEffect(() => { inputRef.current?.focus(); }, []);

// Also: store previous value / timer ids without re-render
const prev = useRef(value);
```

## Memoization (use sparingly)

```tsx
const sorted = useMemo(() => [...items].sort(byName), [items]);
const onSelect = useCallback((id: string) => setSelected(id), []);
```

**When:** expensive pure calc, or stable callbacks for heavily memoized children.  
**Not when:** premature optimization on cheap renders.

## Context

```tsx
const AuthContext = createContext<Auth | null>(null);

function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const value = useMemo(() => ({ user, setUser }), [user]);
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth outside provider');
  return ctx;
}
```

## Interview Q&A

- **Q:** ref vs state?
  **A:** State → UI updates; ref → mutable imperative/non-visual values.
- **Q:** Context performance?
  **A:** All consumers re-render when value changes — split contexts or select carefully.
- **Q:** React Compiler?
  **A:** Can auto-memoize; still understand manual memo for interviews/legacy.

## Pitfalls

- `useMemo` everywhere “for performance”  
- Putting unstable object literals inline in Provider `value`  
- Using context for high-frequency data (mouse coords)

## 60-second answer

“Refs hold mutable non-UI data. Memo/callback stabilize expensive work or child props when measured. Context shares cross-cutting data with awareness of re-render fan-out.”

## Further study

- [React: useRef](https://react.dev/reference/react/useRef) — refs vs state and DOM access.
- [React: useMemo](https://react.dev/reference/react/useMemo) — when memoization helps (and when it doesn’t).
- [React: useCallback](https://react.dev/reference/react/useCallback) — stabilizing function identities.
- [React: Passing Data Deeply with Context](https://react.dev/learn/passing-data-deeply-with-context) — Provider patterns and pitfalls.
- [React: useContext](https://react.dev/reference/react/useContext) — consuming context correctly.

## Practice prompts

1. Build `usePrevious`  
2. Split theme vs auth contexts  
3. Profile before/after useMemo on a large list
