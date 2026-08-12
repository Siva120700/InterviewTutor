---
id: react-security-xss
title: React Security and XSS
track: react
module: "03 Advanced Rendering"
order: 23
languages: [typescript]
summary: How React escapes JSX, dangers of dangerouslySetInnerHTML, and frontend security basics.
---

## Why this matters

XSS in a React app is still XSS. Seniors must know what React protects by default — and what voids that protection.

## Definitions

- **XSS (cross-site scripting):** Injecting attacker script into pages viewed by others.
- **Auto-escaping:** JSX text and most attribute interpolations are escaped by React.
- **dangerouslySetInnerHTML:** Explicit HTML injection API — you own sanitization.
- **Sanitization:** Cleaning HTML with a vetted library (e.g. DOMPurify) before inject.
- **Trusted Types / CSP:** Browser policies that reduce XSS impact — defense in depth.
- **Open redirect / token in URL:** Auth/security footguns adjacent to SPA routing.

## Safe by default

```tsx
const user = '<img src=x onerror=alert(1) />';
return <p>{user}</p>; // rendered as text, not HTML
```

## Dangerous path

```tsx
// Only with sanitized HTML from a trusted pipeline
function Html({ html }: { html: string }) {
  return <div dangerouslySetInnerHTML={{ __html: html }} />;
}
```

Never pass raw user Markdown/HTML without sanitization. Prefer Markdown → React elements via a safe pipeline.

## Other frontend footguns

| Issue | Mitigation |
|-------|------------|
| Storing secrets in frontend | Don’t — use server/BFF |
| `eval` / `new Function` on user data | Never |
| `href={`javascript:${x}`}` | Allowlist protocols (`https:`) |
| Auth tokens in `localStorage` | Prefer httpOnly cookies where possible; know XSS token theft risk |
| User-generated URLs | Validate origin/path |

## Interview Q&A

- **Q:** Does React make XSS impossible?  
  **A:** No — it helps via escaping, but `dangerouslySetInnerHTML`, unsafe URLs, and third-party scripts still bite.
- **Q:** Markdown preview?  
  **A:** Use a sanitizing renderer; never `dangerouslySetInnerHTML={md)}` raw.
- **Q:** CSP role?  
  **A:** Limits script sources even if injection happens — complementary, not a substitute for safe rendering.

## Pitfalls

- “We’re React so we’re safe”  
- Sanitizing on the server then concatenating more unsanitized HTML client-side  
- Disabling escaping in test utilities and copying that into prod

## 60-second answer

“JSX escapes text by default. XSS appears when we inject HTML/URLs unsafely. I avoid `dangerouslySetInnerHTML` unless the HTML is sanitized, and I treat tokens and redirects as part of the threat model.”

## Further study

- Platform API security track · SSR/Next (hydration mismatch caveats)  
- [OWASP XSS](https://owasp.org/www-community/attacks/xss/)

## Practice prompts

1. Find the XSS in a component using `dangerouslySetInnerHTML`  
2. Design a safe blog preview pipeline  
3. Explain why `javascript:` URLs are dangerous in `<a href>`
