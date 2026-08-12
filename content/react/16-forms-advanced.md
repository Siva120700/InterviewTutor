---
id: react-forms-advanced
title: Advanced Forms and Validation
track: react
module: "02 Hooks and Composition"
order: 16
languages: [typescript]
summary: Controlled forms at scale, schema validation, field arrays, and accessible error UX.
---

## Why this matters

CRUD UIs are form UIs. Interviews and take-homes punish messy state, missing a11y errors, and validation only on submit with no field feedback.

## Definitions

- **Controlled input:** React state is the source of truth for the value.
- **Uncontrolled input:** DOM holds value; read via ref (fine for simple/quick forms).
- **Schema validation:** Declare rules once (Zod/Yup) and reuse on client (± server).
- **Touched / dirty:** UX flags — show errors after blur or submit, not on first render.
- **Field array:** Dynamic list of fields (add/remove invitees).
- **Native constraint validation:** `required`, `type="email"` — good baseline, not enough alone.

## Controlled form core

```tsx
type Form = { email: string; age: string };

export function Signup() {
  const [form, setForm] = useState<Form>({ email: '', age: '' });
  const [errors, setErrors] = useState<Partial<Form>>({});

  function onChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
  }

  function validate(f: Form) {
    const next: Partial<Form> = {};
    if (!f.email.includes('@')) next.email = 'Enter a valid email';
    if (Number(f.age) < 18) next.age = 'Must be 18+';
    return next;
  }

  function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    const next = validate(form);
    setErrors(next);
    if (Object.keys(next).length) return;
    // mutate…
  }

  return (
    <form onSubmit={onSubmit} noValidate>
      <label>
        Email
        <input name="email" value={form.email} onChange={onChange} aria-invalid={!!errors.email} />
      </label>
      {errors.email && <p role="alert">{errors.email}</p>}
      {/* age field similarly */}
      <button type="submit">Create</button>
    </form>
  );
}
```

## Schema sketch (Zod)

```tsx
import { z } from 'zod';

const Schema = z.object({
  email: z.string().email(),
  age: z.coerce.number().int().min(18),
});

const parsed = Schema.safeParse(form);
if (!parsed.success) {
  // map parsed.error.flatten().fieldErrors into UI
}
```

Libraries like **React Hook Form** reduce re-renders (uncontrolled + register) while keeping schema resolvers — mention in interviews even if you hand-roll small forms.

## A11y checklist

- Associate `<label htmlFor>` / wrap inputs  
- `aria-invalid` + `aria-describedby` pointing at error text  
- `role="alert"` or live region for submit errors  
- Don’t rely on color alone  

## Interview Q&A

- **Q:** Controlled vs RHF?  
  **A:** Controlled is clear for small forms; RHF/Formik scale better with large field counts and less re-render noise.
- **Q:** Validate on change or submit?  
  **A:** Hybrid — validate on submit always; after first submit or blur, validate on change for that field.
- **Q:** Server errors?  
  **A:** Map API field errors back into the same error shape; keep one error model.

## Pitfalls

- Storing derived errors in effects instead of computing on submit/change  
- Blocking paste/keyboard weirdly with aggressive masks  
- Resetting the whole form state when only one field should clear

## 60-second answer

“I keep a single form model, validate with a schema, surface field errors accessibly, and choose controlled vs RHF based on size. URL or server remains source of truth after successful submit.”

## Further study

- Lists/events/forms · Testing/a11y · Data fetching mutations  

## Practice prompts

1. Multi-step form with persisted draft state  
2. Dynamic “add phone number” field array  
3. Wire Zod errors to `aria-describedby`
