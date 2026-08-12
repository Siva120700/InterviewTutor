---
id: react-context-patterns
title: Context Patterns That Scale
track: react
module: "02 Hooks and Composition"
order: 14
languages: [typescript]
summary: Split contexts, stable providers, dependency injection, and when Context is the wrong tool.
---

## Why this matters

Misused Context becomes a global re-render storm. Seniors design providers like APIs — narrow, stable, and easy to test.

## Definitions

- **Context:** React’s built-in dependency injection for a subtree — not a full state library.
- **Provider:** Component that publishes a context value to descendants.
- **Consumer / useContext:** Read the nearest provider value; subscribe to changes.
- **Context splitting:** Separate high-frequency state from rare config to limit re-renders.
- **Default value:** Fallback when no provider exists — useful for tests, dangerous if it hides missing providers.
- **Provider hell:** Too many nested providers — fix with composition (`composeProviders`) or fewer, clearer boundaries.

## Anti-pattern — one giant value

```tsx
// Re-renders everyone on every keystroke
<AppContext.Provider value={{ user, theme, cart, setQuery, query }}>
  {children}
</AppContext.Provider>
```

## Better — split by update rate

```tsx
const ThemeContext = createContext<'light' | 'dark'>('light');
const AuthContext = createContext<Auth | null>(null);

export function ThemeProvider({ children }: { children: React.ReactNode }) {
  const [theme, setTheme] = useState<'light' | 'dark'>('light');
  const value = useMemo(() => ({ theme, setTheme }), [theme]);
  return <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>;
}
```

Or split **state** and **dispatch** contexts so readers of dispatch don’t re-render on every state change.

## Stable provider value

```tsx
const value = useMemo(() => ({ user, logout }), [user]);
// Avoid: value={{ user, logout }} new object every render
```

## DI for tests

```tsx
function renderWithAuth(ui: React.ReactNode, auth: Auth) {
  return render(<AuthContext.Provider value={auth}>{ui}</AuthContext.Provider>);
}
```

## When not to use Context

| Need | Prefer |
|------|--------|
| Server/cache data | React Query / router loaders |
| High-frequency local UI state | Component state / colocated reducer |
| Cross-app complex updates | Zustand/Redux with selectors |
| Pass prop 1–2 levels | Just prop-drill |

## Interview Q&A

- **Q:** Does Context replace Redux?  
  **A:** For low-frequency shared data (theme, locale, auth user), yes. For complex update graphs, use a store with selectors.
- **Q:** Why memoize context value?  
  **A:** New object identity forces all consumers to re-render even if fields are equal.
- **Q:** Default context vs throwing?  
  **A:** `useAuth()` that throws if missing provider fails fast — often better than silent defaults.

## Pitfalls

- Putting fetch results only in Context without cache keys / request lifecycle  
- Exporting mutable globals beside Context “for convenience”  
- One provider wrapping the entire app for every concern

## 60-second answer

“Context is subtree DI. I split providers by concern and update frequency, keep values referentially stable, and I don’t use Context as a server cache. If many unrelated consumers thrash, I move to a selector-based store.”

## Further study

- State management lesson · Hooks advanced  
- [Passing data deeply with Context](https://react.dev/learn/passing-data-deeply-with-context)

## Practice prompts

1. Split theme vs auth providers  
2. Implement `useAuth` that throws without provider  
3. Show a re-render bug from inline context value and fix it
