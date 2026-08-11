---
id: react-data-fetching
title: Data Fetching and Caching
track: react
module: "04 Senior Architecture"
order: 31
languages: [typescript]
summary: Fetch patterns, race conditions, cache keys, mutations, and optimistic UI for senior interviews.
---

## Why this matters

Frontend system design almost always includes data loading. Seniors own caching and consistency UX.

## Definitions

- **Data fetching:** Loading remote data into the UI with explicit loading, error, cache, and race-handling behavior.
- **Cache key:** Identity of a query (e.g. `['user', id]`)—correctness depends on including tenant/user dimensions.
- **Stale-while-revalidate:** Show cached data immediately, then refresh in the background.
- **Mutation:** A write that should invalidate or update related cached reads.
- **Optimistic update:** Update the UI before the server confirms; roll back if the mutation fails.
- **Race condition:** An older response arrives after a newer request—abort or ignore stale results.
- **Idempotency:** Safe retries for writes using keys/tokens so duplicates don’t double-apply.


## Anti-pattern

```tsx
// Fragile without abort / ignore flag
useEffect(() => {
  fetch(`/api/items/${id}`).then(r => r.json()).then(setData);
}, [id]);
```

## Better sketch

```tsx
useEffect(() => {
  const ac = new AbortController();
  setStatus('loading');
  fetch(`/api/items/${id}`, { signal: ac.signal })
    .then(r => r.json())
    .then(data => { setData(data); setStatus('ok'); })
    .catch(e => { if (e.name !== 'AbortError') setStatus('error'); });
  return () => ac.abort();
}, [id]);
```

Or use TanStack Query for dedupe, retries, focus refetch, invalidation.

## Mutations

```tsx
// Conceptual
await createTodo(input);
queryClient.invalidateQueries({ queryKey: ['todos'] });
```

## Interview Q&A

- **Q:** Where to fetch — component, loader, RSC?
  **A:** Discuss SPA client fetch vs route loaders vs server components (Next). Trade-offs: SEO, waterfalls, caching.
- **Q:** Global spinner vs per-query?
  **A:** Prefer local boundaries; skeletons > blocking the world.
- **Q:** Auth headers?
  **A:** Centralize in fetch client; refresh tokens carefully (no stampede).

## Pitfalls

- Cache key missing user/tenant id → data leaks  
- Optimistic UI without rollback  
- Waterfalls: await A then B when parallel possible

## 60-second answer

“I treat server data as cached queries with explicit keys, abort in-flight races, and invalidate on mutation. Optimistic UI is intentional with rollback.”

## Further study

- [TanStack Query: Overview](https://tanstack.com/query/latest/docs/framework/react/overview) — queries, mutations, and cache keys.
- [React: Synchronizing with Effects](https://react.dev/learn/synchronizing-with-effects) — effect-based fetching pitfalls.
- [MDN: AbortController](https://developer.mozilla.org/en-US/docs/Web/API/AbortController) — canceling in-flight fetches.
- [React: You Might Not Need an Effect](https://react.dev/learn/you-might-not-need-an-effect) — frameworks/libraries vs manual fetch effects.

## Practice prompts

1. Design cache keys for paginated search  
2. Optimistic like button  
3. Avoid waterfall on profile + posts
