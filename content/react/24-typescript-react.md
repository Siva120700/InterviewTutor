---
id: react-typescript
title: TypeScript with React
track: react
module: "03 Advanced Rendering"
order: 24
languages: [typescript]
summary: Typing props, children, events, generics for components, and useful utility patterns.
---

## Why this matters

Strong TypeScript is a senior frontend signal — fewer runtime surprises and clearer component APIs.

## Definitions

- **Props type:** The public input contract of a component.
- **ReactNode / ReactElement:** `ReactNode` is anything renderable; `ReactElement` is a single element object.
- **Event types:** `React.ChangeEvent`, `React.MouseEvent`, etc., parameterized by element type.
- **Generic component:** Component type parameters for reusable lists/selects.
- **Discriminated unions:** Variant props (`type: 'text' | 'image'`) for safe branching.
- **ComponentPropsWithoutRef:** Utility to extend native element props cleanly.

## Props and children

```tsx
type CardProps = {
  title: string;
  children?: React.ReactNode;
};

export function Card({ title, children }: CardProps) {
  return (
    <section>
      <h2>{title}</h2>
      {children}
    </section>
  );
}
```

## Events

```tsx
function onChange(e: React.ChangeEvent<HTMLInputElement>) {
  console.log(e.target.value);
}

function onClick(e: React.MouseEvent<HTMLButtonElement>) {
  e.preventDefault();
}
```

## Extending native elements

```tsx
type ButtonProps = React.ComponentPropsWithoutRef<'button'> & {
  variant?: 'primary' | 'ghost';
};

export function Button({ variant = 'primary', ...rest }: ButtonProps) {
  return <button data-variant={variant} {...rest} />;
}
```

## Generic list

```tsx
type ListProps<T> = {
  items: T[];
  getKey: (item: T) => string;
  renderItem: (item: T) => React.ReactNode;
};

export function List<T>({ items, getKey, renderItem }: ListProps<T>) {
  return <ul>{items.map((item) => <li key={getKey(item)}>{renderItem(item)}</li>)}</ul>;
}
```

## Discriminated props

```tsx
type AlertProps =
  | { kind: 'error'; retry: () => void }
  | { kind: 'info'; retry?: never };

function Alert(props: AlertProps) {
  if (props.kind === 'error') return <button onClick={props.retry}>Retry</button>;
  return <p>Info</p>;
}
```

## Interview Q&A

- **Q:** `FC` type?  
  **A:** Many codebases skip `React.FC` (implicit children history); prefer explicit props.
- **Q:** How to type `useRef` for DOM?  
  **A:** `useRef<HTMLInputElement>(null)` → `ref.current` is `HTMLInputElement | null`.
- **Q:** Typing Context?  
  **A:** `createContext<Auth | null>(null)` + narrow in the hook.

## Pitfalls

- `any` on event handlers “to save time”  
- Optional children vs requiring `children: ReactNode` incorrectly  
- Overusing enums when string unions suffice

## 60-second answer

“I type props explicitly, use element-typed events, extend native props with `ComponentPropsWithoutRef`, and model variants with discriminated unions. Generics keep list/select APIs honest.”

## Further study

- JSX/components · Testing (typed RTL queries)  
- [React TypeScript cheatsheet](https://react-typescript-cheatsheet.netlify.app/)

## Practice prompts

1. Type a polymorphic `Text` component (`as` prop) sketch  
2. Fix a bad `onChange: any`  
3. Generic `Select<T>` with typed `onChange(value: T)`
