---
id: react-props-state
title: Props and State
track: react
module: "01 Foundations"
order: 2
languages: [typescript]
summary: Unidirectional data flow, props vs state, lifting state — core mental model.
---

## Why this matters

Most React bugs are state ownership bugs. Seniors decide *where* state lives and what stays props.

## Definitions

- **Props:** Read-only inputs passed from parent to child to configure a component.
- **State:** Data a component owns that can change over time and triggers a re-render when updated.
- **Unidirectional data flow:** Data flows parent → child via props; children notify parents via callbacks.
- **Lifting state up:** Moving shared state to the nearest common ancestor so siblings stay in sync.
- **Controlled input:** Form value driven entirely by React state plus an `onChange` updater.
- **Derived state:** Values computed from props/state—prefer calculating during render over storing duplicates.
- **Immutable update:** Replace state with a new value/object instead of mutating in place so React can detect changes.


## Concept

```tsx
function Counter() {
  const [count, setCount] = useState(0);
  return (
    <button type="button" onClick={() => setCount(c => c + 1)}>
      Clicked {count}
    </button>
  );
}

type NameFieldProps = {
  value: string;
  onChange: (v: string) => void;
};

function NameField({ value, onChange }: NameFieldProps) {
  return (
    <input
      value={value}
      onChange={e => onChange(e.target.value)}
      aria-label="Name"
    />
  );
}
```

**Rule:** If two siblings need the same data, lift it. If only one component needs it, keep it local.

## Interview Q&A

- **Q:** Props vs state?
  **A:** Props are configured by parent; state is owned and updated internally (or via lifted callbacks).
- **Q:** Why immutable updates?
  **A:** React detects changes by new references/values; mutating hides updates and breaks memoization.
- **Q:** When is `setState` async?
  **A:** Updates are batched; read new state from the updater form `setX(prev => …)` or after render.

## Pitfalls

- Copying props into state and letting them drift  
- Deeply mutating nested objects  
- Prop drilling without composition/context when deep

## 60-second answer

“Props configure; state is owned data that changes. I keep state as low as possible, lift when shared, and update immutably so renders stay predictable.”

## Further study

- [React: Passing Props to a Component](https://react.dev/learn/passing-props-to-a-component) — props as configuration.
- [React: State: A Component's Memory](https://react.dev/learn/state-a-components-memory) — when and how to use state.
- [React: Sharing State Between Components](https://react.dev/learn/sharing-state-between-components) — lifting state up.
- [React: Choosing the State Structure](https://react.dev/learn/choosing-the-state-structure) — avoiding redundant derived state.

## Practice prompts

1. Temperature converter with lifted state  
2. Fix a bug caused by mutating an array in state  
3. Convert an uncontrolled input to controlled
