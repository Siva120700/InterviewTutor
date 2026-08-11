---
id: react-jsx-components
title: JSX and Components
track: react
module: "01 Foundations"
order: 1
languages: [typescript]
summary: What React is, JSX rules, function components, and composition — the senior baseline.
---

## Why this matters

Every React interview starts here. Seniors must explain components as units of UI + state boundary, not “HTML in JS”.

## Definitions

- **React:** A UI library for building interfaces from reusable components with a declarative render model.
- **Component:** A function (or class) that returns a description of UI for given props and state.
- **JSX:** Syntax that looks like HTML but compiles to `React.createElement` / JSX runtime calls—it is JavaScript.
- **Element:** A plain object describing a DOM node or component (`{ type, props }`), not the real DOM node.
- **Composition:** Building complex UI by nesting and combining smaller components instead of inheritance.
- **Declarative UI:** You describe the desired UI for current state; React updates the DOM to match.
- **Pure render:** Rendering should compute UI from props/state without side effects (no network/timers during render).


## Concept

Prefer **function components**. One component ≈ one responsibility (display, layout, or container).

```tsx
type HelloProps = { name: string };

export function Hello({ name }: HelloProps) {
  return <h1 className="title">Hello, {name}</h1>;
}

export function App() {
  return (
    <main>
      <Hello name="Alex" />
    </main>
  );
}
```

### JSX rules seniors mention

- One parent (or Fragment `<>...</>`)
- `className` not `class`, `htmlFor` not `for`
- Expressions in `{ }`; no `if` statements directly inside JSX (use `&&`, ternary, or early vars)
- Lists need stable `key`s (next lessons)

## Interview Q&A

- **Q:** JSX vs HTML?
  **A:** JSX is JS — attributes camelCase, can embed expressions, compiles to function calls.
- **Q:** Class vs function components?
  **A:** Functions + hooks are the modern default; classes still appear in legacy code.
- **Q:** What does render mean?
  **A:** Calling the component function to produce elements; React reconciles that with the DOM.

## Pitfalls

- Mutating props  
- Side effects during render (network, timers)  
- Giant “god” components

## 60-second answer

“React UIs are component trees. Function components return elements from props/state; JSX is declarative sugar. I compose small components and keep render pure.”

## Further study

- [React: Learn React](https://react.dev/learn) — official mental model for components and JSX.
- [React: Your First Component](https://react.dev/learn/your-first-component) — function components and composition basics.
- [React: Writing Markup with JSX](https://react.dev/learn/writing-markup-with-jsx) — JSX rules and differences from HTML.
- [React: Thinking in React](https://react.dev/learn/thinking-in-react) — how to break UI into a component tree.

## Practice prompts

1. Split a page into Header / Content / Footer components  
2. Explain what `createElement` produces  
3. Refactor nested JSX into composed children
