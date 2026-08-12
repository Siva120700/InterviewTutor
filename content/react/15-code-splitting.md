---
id: react-code-splitting
title: Code Splitting and Lazy Loading
track: react
module: "02 Hooks and Composition"
order: 15
languages: [typescript]
summary: React.lazy, Suspense, route-based splitting, and bundler realities for performance interviews.
---

## Why this matters

Large SPAs must not ship the entire app on first load. Seniors talk route-level splits, suspense fallbacks, and measuring real bundles — not only `React.lazy` trivia.

## Definitions

- **Code splitting:** Breaking the JS bundle into chunks loaded on demand.
- **React.lazy:** Dynamic `import()` wrapper for a default-export component.
- **Suspense boundary:** UI placeholder while a lazy child (or other suspender) is loading.
- **Route-based splitting:** Each major route gets its own chunk — highest ROI split.
- **Prefetch:** Loading a chunk before navigation (on hover/visibility) to hide latency.
- **Waterfall:** Lazy component that then lazy-loads data/more code sequentially — avoid when possible.

## Lazy + Suspense

```tsx
import { lazy, Suspense } from 'react';

const SettingsPage = lazy(() => import('./SettingsPage'));

export function AppRoutes() {
  return (
    <Suspense fallback={<p>Loading…</p>}>
      <SettingsPage />
    </Suspense>
  );
}
```

Named exports:

```tsx
const Chart = lazy(() =>
  import('./Chart').then((m) => ({ default: m.Chart })),
);
```

## Route-level pattern

```tsx
const Home = lazy(() => import('./pages/Home'));
const Editor = lazy(() => import('./pages/Editor')); // heavy

<Route
  path="editor"
  element={
    <Suspense fallback={<EditorSkeleton />}>
      <Editor />
    </Suspense>
  }
/>
```

Keep the shell (nav) eager; delay heavy feature routes.

## What to split / not split

| Split | Don’t bother |
|-------|----------------|
| Admin, editor, charts | Tiny buttons/icons |
| Rare settings screens | Above-the-fold shell |
| Heavy markdown/mermaid | Auth provider needed everywhere |

## Interview Q&A

- **Q:** `lazy` vs Next dynamic import?  
  **A:** Same idea; frameworks add SSR-aware loading and sometimes RSC boundaries.
- **Q:** Does splitting always help LCP?  
  **A:** Helps TTI/bundle weight; if the fold needs the chunk, you only deferred pain — measure.
- **Q:** Error if chunk fails to load?  
  **A:** Error boundary around Suspense to show retry UI.

## Pitfalls

- Lazy-loading every tiny component → request spam  
- Suspense fallback that shifts layout (CLS)  
- Forgetting default export when using `lazy`

## 60-second answer

“I split by route and heavy features with `React.lazy` + Suspense, keep the shell eager, and verify with the bundle analyzer. Prefetch on intent when navigations feel laggy.”

## Further study

- Concurrent/Suspense · SSR/Next · Rendering performance  
- [Code splitting](https://react.dev/reference/react/lazy)

## Practice prompts

1. Lazy-load a modal’s heavy body  
2. Add an error boundary retry for failed chunks  
3. Sketch which routes you’d split in a dashboard app
