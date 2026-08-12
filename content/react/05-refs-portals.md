---
id: react-refs-portals
title: Refs, DOM Access, and Portals
track: react
module: "01 Foundations"
order: 5
languages: [typescript]
summary: useRef for DOM and mutable boxes, forwardRef/useImperativeHandle, and portals for modals.
---

## Why this matters

Focus management, measuring layout, integrating non-React widgets, and modals all need refs/portals — frequent UI interview topics.

## Definitions

- **Ref:** A mutable container (`.current`) that persists across renders without causing re-renders when changed.
- **DOM ref:** A ref attached to a host element to call `focus()`, measure, or scroll.
- **forwardRef:** Lets a parent pass a ref through to a child host component.
- **useImperativeHandle:** Customizes the instance value exposed to parents via ref (keep minimal).
- **Portal:** `createPortal(child, domNode)` renders children into a DOM node outside the parent hierarchy while keeping React context.
- **Escape hatch:** Refs/portals step outside declarative flow — use sparingly.

## DOM focus example

```tsx
import { useRef } from 'react';

export function SearchBox() {
  const inputRef = useRef<HTMLInputElement>(null);

  return (
    <>
      <button type="button" onClick={() => inputRef.current?.focus()}>
        Focus
      </button>
      <input ref={inputRef} />
    </>
  );
}
```

## forwardRef + imperative handle

```tsx
import { forwardRef, useImperativeHandle, useRef } from 'react';

export type VideoHandle = { play: () => void; pause: () => void };

export const Video = forwardRef<VideoHandle, { src: string }>(function Video({ src }, ref) {
  const el = useRef<HTMLVideoElement>(null);
  useImperativeHandle(ref, () => ({
    play: () => void el.current?.play(),
    pause: () => el.current?.pause(),
  }));
  return <video ref={el} src={src} />;
});
```

Expose a **small** imperative API — don’t mirror the whole DOM.

## Portal modal

```tsx
import { createPortal } from 'react-dom';

export function Modal({ open, children }: { open: boolean; children: React.ReactNode }) {
  if (!open) return null;
  return createPortal(
    <div className="modal-root" role="dialog" aria-modal="true">
      {children}
    </div>,
    document.body,
  );
}
```

Events still bubble through the **React** tree (not the DOM parent), which matters for outside-click handlers.

## Interview Q&A

- **Q:** Ref vs state?  
  **A:** State → UI updates. Ref → mutable value or DOM node without re-render.
- **Q:** Why portals for modals?  
  **A:** Avoid `overflow: hidden` / z-index clipping; still share React context from the opener.
- **Q:** Callback refs?  
  **A:** `ref={(node) => …}` runs on attach/detach — useful when you need to know mount timing.

## Pitfalls

- Reading `ref.current` during render for display data (should be state)  
- Forgetting to clean up third-party widgets in effects  
- Giant `useImperativeHandle` surfaces that fight React’s model

## 60-second answer

“Refs hold mutable values and DOM nodes without triggering renders. Portals render UI elsewhere in the DOM while keeping React ownership and context — ideal for dialogs. Imperative handles should stay tiny.”

## Further study

- Hooks advanced (`useRef`) · Error boundaries / UI patterns  
- [Portals](https://react.dev/reference/react-dom/createPortal)

## Practice prompts

1. Autofocus an input when a dialog opens  
2. Build a modal with focus trap sketch  
3. Wrap a chart library that needs a DOM node
