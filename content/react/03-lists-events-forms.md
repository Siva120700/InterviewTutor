---
id: react-lists-events-forms
title: Lists, Events, and Forms
track: react
module: "01 Foundations"
order: 3
languages: [typescript]
summary: Keys, synthetic events, controlled forms, and validation patterns for interviews.
---

## Why this matters

List keys and form control are classic trip-up questions — and real prod bug sources.

## Definitions

- **Key:** Stable identity for a list item so React can match the same element across renders and preserve state.
- **Reconciliation:** Diffing previous vs next element trees to decide minimal DOM updates.
- **Synthetic event:** React’s cross-browser event system with a DOM-like API (`onClick`, `onChange`, etc.).
- **Controlled component:** Form element whose value is fully driven by React state.
- **Uncontrolled component:** Form element that keeps value in the DOM; read via refs when needed.
- **Event handler:** Function that runs in response to user input—belongs in handlers, not in effects by default.
- **`preventDefault`:** Stops the browser’s default action (e.g. form navigation) so React can handle submit/validation.


## Lists and keys

```tsx
type User = { id: string; name: string };

function UserList({ users }: { users: User[] }) {
  return (
    <ul>
      {users.map(u => (
        <li key={u.id}>{u.name}</li>
      ))}
    </ul>
  );
}
```

**Never use index as key** if the list can reorder/insert/delete — identity breaks, state jumps rows.

## Forms

```tsx
function LoginForm({ onSubmit }: { onSubmit: (email: string) => void }) {
  const [email, setEmail] = useState('');
  const [error, setError] = useState<string | null>(null);

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!email.includes('@')) {
      setError('Valid email required');
      return;
    }
    setError(null);
    onSubmit(email);
  }

  return (
    <form onSubmit={handleSubmit}>
      <input value={email} onChange={e => setEmail(e.target.value)} />
      {error && <p role="alert">{error}</p>}
      <button type="submit">Continue</button>
    </form>
  );
}
```

## Interview Q&A

- **Q:** Why keys?
  **A:** Help reconciliation preserve component state/DOM for the correct item.
- **Q:** Controlled vs uncontrolled?
  **A:** Controlled for validation/UX sync; uncontrolled for simple or non-React integration.
- **Q:** `preventDefault`?
  **A:** Stops native form navigation so SPA handles submit.

## Pitfalls

- `key={index}` on dynamic lists  
- Forgetting `preventDefault`  
- Storing every keystroke in global store unnecessarily

## 60-second answer

“I use stable ids as keys, prefer controlled forms when I need validation, and handle submit in React with preventDefault. Keys protect identity during list edits.”

## Further study

- [React: Rendering Lists](https://react.dev/learn/rendering-lists) — keys and list reconciliation.
- [React: Responding to Events](https://react.dev/learn/responding-to-events) — event handlers and propagation.
- [React: Sharing State — forms patterns](https://react.dev/learn/reacting-to-input-with-state) — controlled inputs.
- [MDN: HTML forms](https://developer.mozilla.org/en-US/docs/Learn/Forms) — native form semantics React builds on.

## Practice prompts

1. Todo list with add/remove and correct keys  
2. Multi-field form with field-level errors  
3. Explain a bug from index keys after sort
