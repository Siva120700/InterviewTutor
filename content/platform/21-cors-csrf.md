---
id: platform-cors-csrf
title: CORS and CSRF
track: platform
module: "03 Security"
order: 21
languages: [java, csharp]
summary: Browser same-origin rules, CORS configuration, and CSRF defenses for cookie auth.
---

## Why this matters

SPA + API setups constantly misconfigure CORS/CSRF. Interviews love this pair.

## Definitions

- **Same-origin policy:** Browser isolation rule: documents from different scheme/host/port cannot freely read each other’s data.
- **CORS (Cross-Origin Resource Sharing):** Server-declared exceptions that allow browsers to expose cross-origin responses to JS (not an AuthN mechanism).
- **Preflight:** Browser `OPTIONS` check for “non-simple” cross-origin requests before the real call.
- **CSRF (Cross-Site Request Forgery):** Attacker site causes the browser to send the user’s authenticated cookies to your API.
- **SameSite cookie:** Cookie attribute (`Lax`/`Strict`/`None`) that limits cross-site sending and mitigates many CSRF cases.
- **Credentialed CORS:** Requests with cookies/Authorization that require an explicit `Access-Control-Allow-Origin` (never `*`) plus `Allow-Credentials`.
- **Anti-CSRF token:** Unpredictable token the legitimate app sends on state-changing requests so forged cross-site posts fail.


## CORS essentials

```text
Access-Control-Allow-Origin: https://app.example.com   # not * with credentials
Access-Control-Allow-Credentials: true
Access-Control-Allow-Headers: Authorization, Content-Type
```

**Rule:** Reflecting arbitrary `Origin` is dangerous.

## CSRF defenses (cookie sessions)

- `SameSite=Lax` or `Strict`  
- Anti-CSRF tokens on state-changing requests  
- Custom header requirement (`X-Requested-With`) for APIs  
- Prefer authorization headers from memory for SPAs (with XSS discipline) or BFF

## Interview Q&A

- **Q:** Does CORS protect APIs from non-browser clients?
  **A:** No — CORS is browser-enforced only. AuthZ still required.
- **Q:** JWT in Authorization header CSRF?
  **A:** Not sent automatically cross-site like cookies — CSRF risk lower; XSS still matters.

## Pitfalls

- `Allow-Origin: *` + cookies  
- Disabling CSRF “because API” while using cookie auth  
- Forgetting preflight breakages in prod gateways

## 60-second answer

“CORS tells browsers who may call my API; it isn’t auth. With cookie sessions I add SameSite and CSRF tokens. With bearer headers, CSRF is weaker but XSS becomes the main threat.”

## Further study

- [MDN: Cross-Origin Resource Sharing (CORS)](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS) — origins, preflight, and credentialed requests.
- [MDN: Same-origin policy](https://developer.mozilla.org/en-US/docs/Web/Security/Same-origin_policy) — what browsers isolate and why CORS exists.
- [OWASP CSRF Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Cross-Site_Request_Forgery_Prevention_Cheat_Sheet.html) — SameSite, tokens, and defense-in-depth.
- [MDN: SameSite cookies](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie/SameSite) — Lax/Strict/None behavior details.

## Practice prompts

1. Configure CORS for app.example.com → api.example.com  
2. Design CSRF for a cookie-based BFF  
3. Debug a failing preflight
