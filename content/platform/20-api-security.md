---
id: platform-api-security
title: API Security Essentials
track: platform
module: "03 Security"
order: 20
languages: [java, csharp]
summary: OWASP API risks, input validation, IDOR, injection, secrets, and HTTPS — senior checklist.
---

## Why this matters

Auth alone isn’t security. Seniors defend APIs against the usual exploit classes.

## Definitions

- **API security:** Protecting HTTP APIs against abuse and data exposure—AuthN/AuthZ, input handling, transport, and secret hygiene.
- **OWASP API Top 10:** Catalog of common API failure patterns (broken AuthZ, injection, excessive data exposure, etc.).
- **IDOR:** Accessing another user’s object by changing an id without an ownership/tenant check.
- **Injection:** Untrusted input interpreted as code or query (SQL, command, LDAP)—prevent with parameterization and allowlists.
- **XSS:** Injecting script into victims’ browsers; APIs contribute when they store or reflect unsanitized content.
- **SSRF:** Tricking the server into fetching internal or sensitive URLs on the attacker’s behalf.
- **Secret:** Credential or key that must never ship in clients, logs, or git; store in a vault/KMS with rotation.


## Checklist

1. AuthN + AuthZ on every sensitive route  
2. Tenant/user ownership checks (anti-IDOR)  
3. Validate/allowlist inputs; parameterized queries  
4. Least data in responses (no oversharing)  
5. Rate limit auth and expensive endpoints  
6. TLS everywhere; HSTS at edge  
7. Secrets in vault/env — never frontend bundles

```java
// Parameterized — never concatenate SQL
jdbc.query("SELECT * FROM orders WHERE id=? AND tenant_id=?", id, tenantId);
```

```csharp
await db.Orders.SingleOrDefaultAsync(o => o.Id == id && o.TenantId == tenantId);
```

## Interview Q&A

- **Q:** First thing on a new API review?
  **A:** AuthZ on resources + IDOR tests + secret scanning.
- **Q:** Mass assignment?
  **A:** Bind only allowed fields DTO — don’t take entire entity from JSON.
- **Q:** Logging?
  **A:** Never log tokens/passwords; redact PII.

## Pitfalls

- Trusting `X-User-Id` header from clients  
- Verbose errors leaking stack/PII  
- CORS `*` with credentials

## 60-second answer

“I assume clients are hostile: enforce AuthZ per resource, parameterize queries, limit data exposure, rate-limit sensitive routes, and keep secrets off the client.”

## Further study

- [OWASP API Security Top 10](https://owasp.org/API-Security/editions/2023/en/0x11-t10/) — the standard risk list for API interviews.
- [OWASP REST Security Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/REST_Security_Cheat_Sheet.html) — concrete HTTP API hardening checklist.
- [OWASP Injection Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Injection_Prevention_Cheat_Sheet.html) — parameterized queries and input handling.
- [MDN: HTTP security](https://developer.mozilla.org/en-US/docs/Web/Security) — browser-facing security building blocks APIs interact with.

## Practice prompts

1. Threat-model a `/orders/{id}` endpoint  
2. Find mass-assignment bug in a sample DTO  
3. Design secret rotation for JWT signing keys
