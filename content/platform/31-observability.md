---
id: platform-observability
title: Observability for Platform Services
track: platform
module: "04 Reliability"
order: 31
languages: [java, csharp]
summary: Logs, metrics, traces, SLIs/SLOs — how to prove auth, cache, and APIs behave in prod.
---

## Why this matters

You can’t operate JWT, cache, or rate limits blindly. Seniors define signals before incidents.

## Definitions

- **Observability:** Ability to understand system behavior from external signals—logs, metrics, and traces—especially under failure.
- **Log:** Discrete event record for debugging; prefer structured JSON with correlation ids, not free-form prose only.
- **Metric:** Aggregated numeric time series (QPS, latency percentiles, error rate, cache hit ratio).
- **Trace:** A request’s path as spans across services, showing where time and errors occur.
- **SLI (Service Level Indicator):** Quantitative measure of user-visible health (availability, latency, correctness).
- **SLO (Service Level Objective):** Target on an SLI (e.g. 99.9% successful requests in 30 days).
- **Cardinality:** Count of unique label combinations; high-cardinality labels (userId) can break metrics backends.


## What to measure for this track

| Area | Signals |
|------|---------|
| Auth | Login success/fail, token validation failures, lockouts |
| Cache | Hit ratio, latency, stampede locks, memory |
| Limits | 429 count, limited keys, upstream pressure |
| API | Latency p95/p99, 5xx, saturation errors |

## Structured log sketch

```json
{
  "level": "info",
  "msg": "auth.login",
  "userIdHash": "a1b2",
  "result": "fail",
  "reason": "bad_password",
  "traceId": "..."
}
```

Never log raw tokens/passwords.

## Interview Q&A

- **Q:** Three pillars?
  **A:** Logs, metrics, traces — use together.
- **Q:** RED vs USE?
  **A:** RED for request services; USE for resources (utilization, saturation, errors).
- **Q:** Alert on what?
  **A:** SLO burn / user impact — not every 4xx spike.

## Pitfalls

- Unstructured logs you can’t query  
- Metrics with userId labels (cardinality bomb)  
- No dashboards for cache hit ratio after launch

## 60-second answer

“I define SLIs for latency/errors, emit structured logs with trace ids, and track auth failures, cache hit ratio, and 429s so platform features are operable.”

## Further study

- [OpenTelemetry documentation](https://opentelemetry.io/docs/) — vendor-neutral logs/metrics/traces model.
- [Google SRE Workbook: Implementing SLOs](https://sre.google/workbook/implementing-slos/) — SLI/SLO design for production services.
- [Prometheus documentation](https://prometheus.io/docs/introduction/overview/) — metrics, labels, and cardinality pitfalls.
- [Microsoft Learn: Distributed tracing](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing) — correlation across .NET services.

## Practice prompts

1. Dashboard for JWT auth service  
2. Alert policy for cache hit ratio drop  
3. Trace a 401 storm across gateway + API
