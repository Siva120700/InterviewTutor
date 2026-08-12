---
id: react-machine-coding
title: React Machine Coding Interview
track: react
module: "04 Senior Architecture"
order: 36
languages: [typescript]
summary: How to approach frontend machine-coding rounds — component breakdown, state, and timeboxing.
---

## Why this matters

Many companies run a 45–90 minute React build (typeahead, table, modal flow). Process beats perfection.

## Definitions

- **Machine coding:** Timed build of a working UI from a prompt.
- **Vertical slice:** End-to-end thin feature first (ugly but working), then polish.
- **State inventory:** List of values, where they live, and what triggers updates.
- **Acceptance checks:** Explicit must-haves from the prompt (keyboard, debounce, empty states).
- **Timebox:** Fixed minutes per phase so you don’t gold-plate CSS.

## 60-minute playbook

| Minutes | Focus |
|--------:|--------|
| 0–5 | Restate requirements + edge cases aloud |
| 5–10 | Component tree + state inventory |
| 10–40 | Vertical slice: data → UI → interactions |
| 40–50 | Edge cases, a11y basics, loading/error |
| 50–60 | Cleanup, rename, speak tradeoffs |

## State inventory example (typeahead)

```text
query: string                         → search input
results: Item[]                       → fetch/cache
status: idle|loading|error|success    → network
open: boolean                         → listbox visible
activeIndex: number                   → keyboard highlight
```

## Component breakdown

```text
SearchPage
  SearchInput (value, onChange, onKeyDown)
  SuggestionList (items, activeIndex, onSelect)
  StatusLine (status)
```

Keep fetch in the page or a `useSearch(query)` hook — not inside every row.

## Debounce sketch

```tsx
function useDebounced<T>(value: T, ms: number) {
  const [v, setV] = useState(value);
  useEffect(() => {
    const t = setTimeout(() => setV(value), ms);
    return () => clearTimeout(t);
  }, [value, ms]);
  return v;
}
```

## What interviewers watch

- Clarity of components and names  
- Correct controlled inputs  
- Race-safe fetch (ignore stale / abort)  
- Keyboard + focus basics  
- Honest about what’s unfinished  

## Interview Q&A

- **Q:** CSS not perfect?  
  **A:** Say you’ll use spacing tokens later — prioritize behavior.
- **Q:** No library allowed?  
  **A:** Hand-roll debounce and list; mention what you’d use in prod.
- **Q:** Stuck on a bug?  
  **A:** Narrate hypotheses; add a quick log; simplify props.

## Pitfalls

- Building a design system before the happy path works  
- Fetching without handling rapid typing  
- Silent empty states  

## 60-second answer

“I restate acceptance criteria, sketch components and state, ship a vertical slice, then harden edges and a11y. I narrate tradeoffs and leave the code easy to extend.”

## Further study

- Data fetching · Forms · Testing/a11y · Routing  

## Practice prompts

1. Typeahead with keyboard selection (60 min)  
2. Sortable/filterable table  
3. Modal form that edits a list row
