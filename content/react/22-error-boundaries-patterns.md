---
id: react-error-boundaries-patterns
title: Error Boundaries and UI Patterns
track: react
module: "03 Advanced"
order: 22
languages: [typescript]
summary: Error boundaries, compound components, controlled patterns, and headless composition.
---

## Why this matters

Seniors design resilient UI and reusable APIs — not only hooks.

## Definitions

- **Error boundary:** A class component using `getDerivedStateFromError` / `componentDidCatch` that catches render errors in its subtree and shows fallback UI.
- **Render-phase errors:** Boundaries catch errors during render/lifecycle of children—not event handlers or async code (those need try/catch).
- **Compound components:** Related components that share implicit state via context (Tabs + TabList + TabPanel).
- **Headless component / hook:** Logic and state without mandated markup—consumers own styling and DOM structure.
- **Portal:** Rendering children into a DOM node outside the parent hierarchy (typical for modals/dialogs).
- **Composition over configuration:** Prefer nesting and slots over giant prop bags for reusable UI.
- **Recovery UI:** Fallback that lets users retry or navigate away instead of a blank white screen.


## Error boundary sketch

```tsx
class ErrorBoundary extends React.Component<
  { fallback: React.ReactNode; children: React.ReactNode },
  { hasError: boolean }
> {
  state = { hasError: false };
  static getDerivedStateFromError() { return { hasError: true }; }
  componentDidCatch(err: unknown) { console.error(err); }
  render() {
    return this.state.hasError ? this.props.fallback : this.props.children;
  }
}
```

Note: event handlers and async errors need their own try/catch; boundaries catch **render/lifecycle** errors.

## Compound pattern

```tsx
const TabsCtx = createContext<TabsApi | null>(null);

export function Tabs({ children }: { children: React.ReactNode }) {
  const [active, setActive] = useState(0);
  const api = useMemo(() => ({ active, setActive }), [active]);
  return <TabsCtx.Provider value={api}>{children}</TabsCtx.Provider>;
}
Tabs.List = function List(/* … */) { /* … */ };
Tabs.Panel = function Panel(/* … */) { /* … */ };
```

## Interview Q&A

- **Q:** Why still class for error boundaries?
  **A:** No hook equivalent yet for catching render errors (as of common interview baseline).
- **Q:** Modal accessibility?
  **A:** Focus trap, `role="dialog"`, Escape, restore focus — portals help stacking.
- **Q:** Compound vs props soup?
  **A:** Compound scales flexible markup; props API is simpler for small cases.

## Pitfalls

- One app-wide boundary only — hard to recover  
- Catching errors and swallowing silently  
- Over-abstracting compounds for one-off UI

## 60-second answer

“I wrap risky subtrees with error boundaries and design reusable UI via composition — compounds or headless hooks — instead of giant prop bags.”

## Further study

- [React: Error Boundaries](https://react.dev/reference/react/Component#catching-rendering-errors-with-an-error-boundary) — official boundary API and limits.
- [React: createPortal](https://react.dev/reference/react-dom/createPortal) — portals for overlays.
- [React: Passing Data with Context](https://react.dev/learn/passing-data-deeply-with-context) — foundation for compound components.
- [WAI-ARIA APG: Dialog Pattern](https://www.w3.org/WAI/ARIA/apg/patterns/dialog-modal/) — accessible modal expectations for portal UI.

## Practice prompts

1. Boundary around a widget with retry  
2. Build a headless `useDisclosure` for modal/accordion  
3. Portal-based dialog with focus restore
