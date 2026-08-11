---
id: react-testing-a11y
title: Testing and Accessibility
track: react
module: "04 Senior Architecture"
order: 32
languages: [typescript]
summary: RTL testing strategy, what to test, and a11y expectations for senior frontends.
---

## Why this matters

Seniors ship accessible UI and tests that survive refactors — not snapshot spam.

## Definitions

- **React Testing Library (RTL):** Test library that encourages querying by roles/text as users and assistive tech would.
- **Unit vs integration vs e2e:** Component logic → multi-component flows → full browser paths; pick by risk, not dogma.
- **Accessibility (a11y):** Interfaces usable via keyboard and assistive tech, with semantic HTML and sufficient contrast.
- **ARIA:** Attributes that expose roles/states/properties when native HTML semantics are insufficient.
- **Focus management:** Moving and restoring focus for modals, drawers, and route changes—critical for keyboard users.
- **Accessible name:** The computed name of a control (label text, `aria-label`) used by `getByRole` and screen readers.
- **Semantic HTML first:** Prefer `button`, `a`, `label`, headings—add ARIA only when native elements can’t express the pattern.


## Testing style

```tsx
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

test('submits email', async () => {
  const user = userEvent.setup();
  const onSubmit = vi.fn();
  render(<LoginForm onSubmit={onSubmit} />);
  await user.type(screen.getByLabelText(/email/i), 'a@b.com');
  await user.click(screen.getByRole('button', { name: /continue/i }));
  expect(onSubmit).toHaveBeenCalledWith('a@b.com');
});
```

**Prefer:** `getByRole`, `findBy*`, user-event.  
**Avoid:** testing state variables, shallow enzyme-style internals.

## A11y checklist (interview)

- Semantic HTML first (`button`, `label`, `nav`)  
- Keyboard path for all actions  
- `aria-live` for async errors  
- Don’t disable zoom; manage focus in dialogs

## Interview Q&A

- **Q:** What do you test?
  **A:** Behavior and contracts; mock network at boundary; e2e for critical journeys.
- **Q:** Snapshot tests?
  **A:** Sparse — brittle; use for stable pure output at most.
- **Q:** a11y tooling?
  **A:** eslint-plugin-jsx-a11y, axe in CI, manual keyboard pass.

## Pitfalls

- Testing implementation details  
- `div` with onClick and no keyboard support  
- Autofocus fights with AT

## 60-second answer

“I test user-facing behavior with RTL and cover critical paths with e2e. Accessibility starts with semantic HTML, keyboard support, and correct roles/focus — ARIA only when needed.”

## Further study

- [React Testing Library intro](https://testing-library.com/docs/react-testing-library/intro/) — user-centric testing philosophy.
- [Testing Library: Guiding Principles](https://testing-library.com/docs/guiding-principles/) — “resemble how users use your software.”
- [WAI-ARIA Authoring Practices Guide (APG)](https://www.w3.org/WAI/ARIA/apg/) — keyboard and ARIA patterns for widgets.
- [MDN: Accessibility](https://developer.mozilla.org/en-US/docs/Web/Accessibility) — foundations of web a11y.

## Practice prompts

1. Write RTL tests for a combobox  
2. Make a custom dropdown keyboard-accessible  
3. Add axe check to CI
