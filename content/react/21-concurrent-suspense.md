---
id: react-concurrent-suspense
title: Concurrent React and Suspense
track: react
module: "03 Advanced"
order: 21
languages: [typescript]
summary: Concurrent rendering, transitions, Suspense boundaries, and what seniors should say in 2024+.
---

## Why this matters

Modern React interviews expect familiarity with concurrent features — even if your app uses them lightly.

## Definitions

- **Concurrent rendering:** React can prepare updates in the background and interrupt work so urgent input stays responsive.
- **Transition:** A non-urgent update marked with `startTransition` / `useTransition` that may be delayed without blocking typing.
- **Suspense:** Declarative loading UI while a child is waiting on async work (data or code) that the data layer supports.
- **useDeferredValue:** Defers updating a derived value so urgent UI (like an input) can paint first.
- **Streaming SSR:** Server sends HTML in chunks as Suspense boundaries resolve (framework-dependent).
- **Priority:** Urgent updates (text input) vs non-urgent (filtering a huge list)—concurrency exists to separate them.
- **Pending UI:** `isPending` from `useTransition` signals that a transition is in flight so you can show subtle feedback.


## Transitions

```tsx
const [isPending, startTransition] = useTransition();
const [query, setQuery] = useState('');
const [list, setList] = useState<Item[]>([]);

function onChange(e: React.ChangeEvent<HTMLInputElement>) {
  const q = e.target.value;
  setQuery(q); // urgent — keep input responsive
  startTransition(() => {
    setList(filterHuge(q)); // non-urgent
  });
}
```

## Suspense (conceptual)

```tsx
<Suspense fallback={<Spinner />}>
  <UserPanel id={id} />
</Suspense>
```

Data libraries (Relay, React Query with suspense mode, framework loaders) integrate with Suspense — don’t invent ad-hoc throw promises unless you know the protocol.

## Interview Q&A

- **Q:** Transition vs debounce?
  **A:** Debounce delays work; transition keeps urgency ranking inside React’s scheduler.
- **Q:** Error vs Suspense?
  **A:** Suspense = wait; Error Boundary = failure. Often wrap both.
- **Q:** Do I need Concurrent for every app?
  **A:** It’s the default architecture; use transitions where interactions feel janky.

## Pitfalls

- Marking everything as transition  
- Suspense without a real data integration story  
- Ignoring UX of pending states (`isPending`)

## 60-second answer

“Concurrent React can interrupt rendering. I keep keystrokes urgent and mark heavy UI updates as transitions. Suspense declares loading boundaries for async UI when the data layer supports it.”

## Further study

- [React: useTransition](https://react.dev/reference/react/useTransition) — marking non-urgent updates.
- [React: useDeferredValue](https://react.dev/reference/react/useDeferredValue) — deferring expensive derived UI.
- [React: Suspense](https://react.dev/reference/react/Suspense) — loading boundaries for async UI.
- [React: Separating Events from Effects](https://react.dev/learn/separating-events-from-effects) — mental model that pairs with transitions.

## Practice prompts

1. Search box with `useTransition`  
2. Nest Suspense for page shell vs details  
3. Compare deferred value vs transition for filtered lists
