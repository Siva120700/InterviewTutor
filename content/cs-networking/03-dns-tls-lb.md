---
id: net-dns-tls-lb
title: DNS, TLS, and Load Balancers
track: cs-networking
module: "02 Infrastructure"
order: 3
languages: [java, csharp]
summary: Resolution, certificates, L4/L7 balancing, and health checks — CS networking advanced basics.
---

## Why this matters

Production debugging and HLD both require DNS/TLS/LB fluency.

## Definitions

- **DNS:** Resolves human-readable names to records (A/AAAA, CNAME, …) with TTL-based caching along the resolution path.
- **TTL (DNS):** How long resolvers may cache an answer before re-querying — stuck TTLs explain “old IP” outages.
- **TLS:** Encrypts a connection and authenticates the server (and optionally the client) using certificates.
- **Certificate:** Signed binding of a public key to an identity (hostname/service) presented in the TLS handshake.
- **SNI (Server Name Indication):** TLS extension naming the target hostname so the server can pick the right certificate.
- **L4 load balancer:** Distributes by IP/port/TCP without inspecting HTTP (fast, less app-aware).
- **L7 load balancer:** Routes on HTTP host/path/headers and often terminates TLS (feature-rich, more CPU).
- **Health check:** Periodic probes that mark backends healthy/unhealthy and enable drain during deploys.

## DNS

Client → resolver → auth nameservers. Records: A/AAAA, CNAME, MX, TXT, TTL caching.

## TLS

Handshake establishes secrets; certs prove identity. Termination often at LB; mTLS for service identity.

## Load balancers

| Type | Layer | Notes |
|------|-------|-------|
| L4 | TCP | Fast, less app-aware |
| L7 | HTTP | Path/host routing, WAF |
| DNS LB | Geo/simple | Coarse |

Health checks + connection draining for deploys.

## Interview Q&A

- **Q:** Sticky sessions?
  **A:** Prefer stateless JWTs/shared session store; sticky as last resort.
- **Q:** Cert rotation?
  **A:** Automation (ACME); overlap validity windows.

## 60-second answer

“DNS finds endpoints, TLS secures the pipe, LBs spread traffic with health checks. I terminate TLS at the edge carefully and keep apps as stateless as possible.”

## Further study

- [DNS (MDN Glossary)](https://developer.mozilla.org/en-US/docs/Glossary/DNS) — name resolution basics and record types
- [TLS (MDN Glossary)](https://developer.mozilla.org/en-US/docs/Glossary/TLS) — encryption and certificate authentication on the wire
- [Transport Layer Security (MDN)](https://developer.mozilla.org/en-US/docs/Web/Security/Transport_Layer_Security) — practical TLS concepts for web services
- [Load balancing overview](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/load-balancing-overview) — L4 vs L7 choices in real architectures

## Practice prompts

1. Trace failure: DNS TTL stuck on old IP  
2. Design blue/green with LB drains  
3. Explain SNI
