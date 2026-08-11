---
id: platform-authorization-rbac
title: Authorization Models — RBAC and Beyond
track: platform
module: "01 Identity"
order: 4
languages: [java, csharp]
summary: RBAC, claims, ABAC, resource checks, and multi-tenant authorization.
---

## Why this matters

“Admin role” isn’t enough for senior systems. You need resource-level and tenant-aware AuthZ.

## Definitions

- **RBAC (Role-Based Access Control):** Users are assigned roles; roles grant permissions; checks ask “does this role allow this action?”
- **Permission:** A fine-grained capability such as `orders:refund` or `users:admin`—prefer naming actions, not only “roles.”
- **ABAC (Attribute-Based Access Control):** Policy evaluated over attributes of user, resource, action, and environment (e.g. owner + business hours).
- **ReBAC:** Relationship-based authorization (“user is editor of doc”) in the style of Google Zanzibar.
- **Least privilege:** Grant only the minimum rights needed for the job; default deny.
- **Policy enforcement point (PEP):** Where the check runs—middleware, gateway, or handler—never only in the UI.
- **IDOR:** Insecure Direct Object Reference—accessing another tenant/user’s resource by guessing an id when ownership isn’t checked.


## RBAC example

```java
enum Permission { ORDER_READ, ORDER_REFUND, USER_ADMIN }

boolean allow(User u, Permission p) {
  return u.roles().stream().anyMatch(r -> r.grants(p));
}

// Resource-level
boolean canRefund(User u, Order o) {
  return allow(u, Permission.ORDER_REFUND)
      && (o.tenantId().equals(u.tenantId()));
}
```

```csharp
[Authorize(Policy = "OrderRefund")]
public async Task<IActionResult> Refund(Guid orderId) { /* also load order + tenant check */ }
```

## Multi-tenant rule

**Always** scope queries by `tenant_id` from the trusted principal — never from a free-form client field alone.

## Interview Q&A

- **Q:** Roles in JWT vs lookup?
  **A:** Embed stable roles for speed; fetch fine-grained perms from DB when they change often.
- **Q:** 403 vs hide resource?
  **A:** Sometimes return 404 to avoid leaking existence — product/security choice.
- **Q:** When ABAC?
  **A:** When rules depend on resource attributes/time/location beyond static roles.

## Pitfalls

- Client-supplied `role=admin` trusted blindly  
- Missing tenant filters (IDOR)  
- Giant role explosion without permission catalog

## 60-second answer

“I map users → roles → permissions, then enforce resource and tenant checks on the server. For richer rules I add ABAC/relationships, always with least privilege.”

## Further study

- [OWASP Authorization Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Authorization_Cheat_Sheet.html) — enforcement patterns and IDOR prevention.
- [OWASP API Security Top 10](https://owasp.org/API-Security/editions/2023/en/0x11-t10/) — broken object-level authorization as a top risk.
- [Microsoft Learn: Role-based authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles) — roles and policies in ASP.NET Core.
- [NIST RBAC](https://csrc.nist.gov/projects/role-based-access-control) — classic RBAC model terminology.

## Practice prompts

1. Model perms for a hospital app (doctor/nurse/admin)  
2. Find IDOR in a sample API  
3. Design feature-flag + role gating
